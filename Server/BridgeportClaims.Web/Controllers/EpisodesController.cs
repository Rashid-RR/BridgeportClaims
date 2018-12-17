using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.EpisodeNotes;
using BridgeportClaims.Data.DataProviders.Episodes;
using BridgeportClaims.Data.DataProviders.Users;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Web.Framework.Models;
using Microsoft.AspNet.Identity;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
	[Authorize(Roles = "User")]
	[RoutePrefix("api/episodes")]
	public class EpisodesController : BaseApiController
	{
        #region Private Members

        private readonly Lazy<IEpisodesDataProvider> _episodesDataProvider;
		private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);
		private readonly Lazy<IEpisodeNoteProvider> _episodeNoteProvider;
		private readonly Lazy<IUsersProvider> _usersProvider;

        #endregion

        #region Ctor

        public EpisodesController(
			Lazy<IEpisodesDataProvider> episodesDataProvider, 
			Lazy<IEpisodeNoteProvider> episodeNoteProvider,
			Lazy<IUsersProvider> usersProvider)
		{
			_episodesDataProvider = episodesDataProvider;
			_episodeNoteProvider = episodeNoteProvider;
			_usersProvider = usersProvider;
		}

        #endregion

        #region Action Methods

        [HttpPost]
		[Route("associate-to-claim")]
		public IHttpActionResult AssociateEpisodeToClaim(EpisodeClaimModel model)
		{
			try
			{
				if (null == model)
					throw new ArgumentNullException(nameof(model));
				if (model.ClaimId == default (int))
					throw new Exception($"Invalid Claim Id {model.ClaimId}.");
				if (model.EpisodeId == default(int))
					throw new Exception($"Invalid Episode Id {model.EpisodeId}.");
				_episodesDataProvider.Value.AssociateEpisodeToClaim(model.EpisodeId, model.ClaimId);
				return Ok(new {message = $"Episode Id {model.EpisodeId} was associated to Claim Id {model.ClaimId} successfully."});
			}
			catch (Exception ex)
			{
				Logger.Value.Error(ex);
				return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
			}
		}

		[HttpPost]
		[Route("save-note")]
		public IHttpActionResult SaveEpisodeNote(SaveEpisodeNoteModel model)
		{
			try
			{
				var userId = User.Identity.GetUserId();
				if (null == userId)
					throw new Exception($"Error, could not retrieve the user.");
				var today = DateTime.UtcNow.ToMountainTime();
				_episodesDataProvider.Value.SaveEpisodeNote(model.EpisodeId, model.Note, userId, today);
				var userDto = _usersProvider.Value.GetUser(userId);
				return Ok(new
				{
					message = "The episode note was saved successfully.",
					Owner = $"{userDto.FirstName} {userDto.LastName}",
					Created = today
				});
			}
			catch (Exception ex)
			{
				Logger.Value.Error(ex);
				return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
			}
		}

		[HttpPost]
		[Route("acquire")]
		public IHttpActionResult AcquireEpisode(int episodeId)
		{
			try
			{
				var userId = User.Identity.GetUserId();
				if (null == userId)
					throw new Exception($"Error, could not retrieve the user.");
				_episodesDataProvider.Value.AssignOrAcquireEpisode(episodeId, userId, userId);
			    var userDto = _usersProvider.Value.GetUser(userId);
				return Ok(new
				{
					message = "The episode was acquired successfully.",
					Owner = $"{userDto.FirstName} {userDto.LastName}"
				});
			}
			catch (Exception ex)
			{
				Logger.Value.Error(ex);
				return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
			}
		}

		[HttpPost]
		[Route("assign")]
		public IHttpActionResult AssignEpisode(int episodeId, string userId)
		{
			try
			{
				var modifiedByUserId = User.Identity.GetUserId();
				if (userId.IsNullOrWhiteSpace())
					throw new ArgumentNullException(nameof(userId));
				_episodesDataProvider.Value.AssignOrAcquireEpisode(episodeId, userId, modifiedByUserId);
			    var userDto = _usersProvider.Value.GetUser(userId);
				return Ok(new
				{
					message = "The episode was assigned successfully.",
					Owner = $"{userDto.FirstName} {userDto.LastName}"
				});
			}
			catch (Exception ex)
			{
				Logger.Value.Error(ex);
				return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
			}
		}

		[HttpPost]
		[Route("note-modal")]
		public IHttpActionResult GetEpisodeNotes(int episodeId)
		{
			try
			{
				var results = _episodeNoteProvider.Value.GetEpisodeNotes(episodeId)?.GroupBy(q => new
					{
						q.Id,
						q.ClaimNumber,
						q.EpisodeCreated,
						q.Owner,
						q.PatientName
					})
					.Select(gcs => new EpisodeNoteHeaderModel
					{
						Id = gcs.Key.Id,
						Owner = gcs.Key.Owner,
						EpisodeCreated = gcs.Key.EpisodeCreated,
						PatientName = gcs.Key.PatientName,
						ClaimNumber = gcs.Key.ClaimNumber,
						EpisodeNotes = gcs.Select(x => new EpisodeNoteModel
							{
								NoteCreated = x.NoteCreated,
								NoteText = x.NoteText,
								WrittenBy = x.WrittenBy
							})
							.ToList()
					});
				return Ok(results);
			}
			catch (Exception ex)
			{
				Logger.Value.Error(ex);
				return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
			}
		}

		[HttpPost]
		[Route("save")]
		public IHttpActionResult SaveNewEpisode(NewEpisodeViewModel model)
		{
			try
			{
				var userId = User.Identity.GetUserId();
				var retVal = new NewEpisodeSaveDto
				{
					Episode = _episodesDataProvider.Value.SaveNewEpisode(model.ClaimId, model.EpisodeTypeId,
						model.PharmacyNabp, model.RxNumber, model.EpisodeText, userId),
					Message = "The episode was saved successfully"
				};
				return Ok(retVal);
			}
			catch (Exception ex)
			{
				Logger.Value.Error(ex);
				return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
			}
		}

		[HttpPost]
		[Route("resolve")]
		public IHttpActionResult MarkEpisodeAsResolved(int episodeId)
		{
			try
			{
				var userId = User.Identity.GetUserId();
				_episodesDataProvider.Value.ResolveEpisode(episodeId, userId);
				return Ok(new {message = "The episode was resolved successfully."});
			}
			catch (Exception ex)
			{
				Logger.Value.Error(ex);
				return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
			}
		}

		[HttpPost]
		[Route("archive")]
		public IHttpActionResult ArchiveEpisode(int episodeId)
		{
			try
			{
				_episodesDataProvider.Value.ArchiveEpisode(episodeId);
				return Ok(new {message = $"Episode Id #{episodeId} was archived successfully."});
			}
			catch (Exception ex)
			{
				Logger.Value.Error(ex);
				return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
			}
		}

		[HttpPost]
		[Route("get")]
		public IHttpActionResult GetEpisodes(EpisodesViewModel m)
		{
			try
			{
				var userId = User.Identity.GetUserId();
				if (null == userId)
					throw new Exception("Error, could not find logged in user.");
				var results = _episodesDataProvider.Value.GetEpisodes(m.StartDate.ToNullableFormattedDateTime(),
					m.EndDate.ToNullableFormattedDateTime(), m.Resolved, m.OwnerId,
					m.EpisodeCategoryId, m.EpisodeTypeId, m.SortColumn, m.SortDirection, m.PageNumber, m.PageSize, userId, m.Archived);
				return Ok(results);
			}
			catch (Exception ex)
			{
				Logger.Value.Error(ex);
				return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
			}
		}

		[HttpGet]
		[Route("getepisodetypes")]
		[AllowAnonymous]
		public IHttpActionResult GetEpisodeTypes()
		{
			try
			{
				return Ok(_episodesDataProvider.Value.GetEpisodeTypes());
			}
			catch (Exception ex)
			{
				Logger.Value.Error(ex);
				return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
			}
		}

        #endregion
    }
}
