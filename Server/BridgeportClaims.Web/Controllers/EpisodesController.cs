﻿using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.EpisodeNotes;
using BridgeportClaims.Data.DataProviders.Episodes;
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

        public EpisodesController(IEpisodesDataProvider episodesDataProvider, IEpisodeNoteProvider episodeNoteProvider)
        {
            _episodesDataProvider = episodesDataProvider;
            _episodeNoteProvider = episodeNoteProvider;
        }

	    [HttpPost]
	    [Route("acquire")]
	    public IHttpActionResult AcquireEpisode(int episodeId)
	    {
            try
            {
                var userId = User.Identity.GetUserId();
                _episodesDataProvider.AcquireEpisode(episodeId, userId);
                return Ok(new {message = "The episode was acquired successfully."});
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
	            var query = _episodeNoteProvider.GetEpisodeNotes(episodeId);
	            var results = query.GroupBy(q => new
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
	                _episodesDataProvider.SaveNewEpisode(model.ClaimId, model.EpisodeTypeId, model.PharmacyNabp, model.RxNumber, model.EpisodeText, userId);
	                return Ok(new {message = "The episode was saved successfully"});
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
	                var results = _episodesDataProvider.GetEpisodes(m.StartDate, m.EndDate, m.Resolved, m.OwnerId,
	                    m.EpisodeCategoryId, m.EpisodeTypeId, m.SortColumn, m.SortDirection, m.PageNumber, m.PageSize);
	                return Ok(results);
                });
	        }
	        catch (Exception ex)
	        {
	            Logger.Error(ex);
	            return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
	    }

		[HttpPost]
		[Route("saveepisode")]
		public async Task<IHttpActionResult> AddOrUpdateEpisode([FromBody] SaveEpisodeModel model)
		{
			try
			{
				return await Task.Run(() =>
				{
					if (!ModelState.IsValid)
						return GetBadRequestFormattedErrorMessages();
					var userId = User.Identity.GetUserId();
					if (null == userId)
						throw new ArgumentNullException(nameof(userId));
					_episodesDataProvider.AddOrUpdateEpisode(model.EpisodeId, model.ClaimId, userId, model.NoteText,
						model.EpisodeTypeId);
					return Ok(new { message = "Episode was saved Successfully" });
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
