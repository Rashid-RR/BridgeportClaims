using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.EpisodeNotes;
using BridgeportClaims.Data.DataProviders.Episodes;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Entities.DomainModels;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
	[Authorize(Roles = "User")]
	[RoutePrefix("api/episodes")]
	public class EpisodesController : BaseApiController
	{
		private readonly IEpisodesDataProvider _episodesDataProvider;
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		private readonly IEpisodeNoteProvider _episodeNoteProvider;
		private readonly IRepository<AspNetUsers> _usersRepository;

		public EpisodesController(IEpisodesDataProvider episodesDataProvider, IEpisodeNoteProvider episodeNoteProvider, IRepository<AspNetUsers> usersRepository)
		{
			_episodesDataProvider = episodesDataProvider;
			_episodeNoteProvider = episodeNoteProvider;
			_usersRepository = usersRepository;
		}

		[HttpPost]
		[Route("save-note")]
		public async Task<IHttpActionResult> SaveEpisodeNote(SaveEpisodeNoteModel model)
		{
			try
			{
				return await Task.Run(() =>
				{
					var user = _usersRepository.Get(User.Identity.GetUserId());
					if (null == user)
						throw new Exception($"Error, could not retrieve the user {User.Identity.Name}");
					var today = DateTime.UtcNow.ToMountainTime();
					_episodesDataProvider.SaveEpisodeNote(model.EpisodeId, model.Note, user.Id, today);
					return Ok(new
					{
						message = "The episode note was saved successfully.",
						Owner = $"{user.FirstName} {user.LastName}",
						Created = today
					});
				});
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
			}
		}

		[HttpPost]
		[Route("acquire")]
		public IHttpActionResult AcquireEpisode(int episodeId)
		{
			try
			{
				var user = _usersRepository.Get(User.Identity.GetUserId());
				if (null == user)
					throw new Exception($"Error, could not retrieve the user {User.Identity.Name}");
				_episodesDataProvider.AssignOrAcquireEpisode(episodeId, user.Id);
				return Ok(new
				{
					message = "The episode was acquired successfully.",
					Owner = $"{user.FirstName} {user.LastName}"
				});
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
			}
		}

		[HttpPost]
		[Route("assign")]
		public async Task<IHttpActionResult> AssignEpisode(int episodeId, string userId)
		{
			try
			{
				return await Task.Run(() =>
				{
					var user = _usersRepository.Get(userId);
					if (null == user)
						throw new Exception($"Error, could not retrieve the user from Id: {userId}");
					_episodesDataProvider.AssignOrAcquireEpisode(episodeId, userId);
					return Ok(new
					{
						message = "The episode was assigned successfully.",
						Owner = $"{user.FirstName} {user.LastName}"
					});
				});
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
			}
		}

		[HttpPost]
		[Route("note-modal")]
		public IHttpActionResult GetEpisodeNotes(int episodeId)
		{
			try
			{
				var results = _episodeNoteProvider.GetEpisodeNotes(episodeId)?.GroupBy(q => new
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
				Logger.Error(ex);
				return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
			}
		}

		[HttpPost]
		[Route("save")]
		public async Task<IHttpActionResult> SaveNewEpisode(NewEpisodeViewModel model)
		{
			try
			{
				return await Task.Run(() =>
				{
					var userId = User.Identity.GetUserId();
					var retVal = new NewEpisodeSaveDto
					{
						Episode = _episodesDataProvider.SaveNewEpisode(model.ClaimId, model.EpisodeTypeId,
							model.PharmacyNabp, model.RxNumber, model.EpisodeText, userId),
						Message = "The episode was saved successfully"
					};
					return Ok(retVal);
				});
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
			}
		}

		[HttpPost]
		[Route("resolve")]
		public async Task<IHttpActionResult> MarkEpisodeAsResolved(int episodeId)
		{
			try
			{
				return await Task.Run(() =>
				{
					var userId = User.Identity.GetUserId();
					_episodesDataProvider.ResolveEpisode(episodeId, userId);
					return Ok(new {message = "The episode was resolved successfully."});
				});
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
			}
		}
		

		[HttpPost]
		[Route("get")]
		public async Task<IHttpActionResult> GetEpisodes(EpisodesViewModel m)
		{
			try
			{
				return await Task.Run(() =>
				{
					var userId = User.Identity.GetUserId();
					if (null == userId)
						throw new Exception("Error, could not find logged in user.");
					var results = _episodesDataProvider.GetEpisodes(m.StartDate.ToNullableFormattedDateTime(), m.EndDate.ToNullableFormattedDateTime(), m.Resolved, m.OwnerId,
						m.EpisodeCategoryId, m.EpisodeTypeId, m.SortColumn, m.SortDirection, m.PageNumber, m.PageSize, userId);
					return Ok(results);
				});
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
			}
		}

		[HttpGet]
		[Route("getepisodetypes")]
		[AllowAnonymous]
		public async Task<IHttpActionResult> GetEpisodeTypes()
		{
			try
			{
				return await Task.Run(() => Ok(_episodesDataProvider.GetEpisodeTypes()));
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
			}
		}
	}
}
