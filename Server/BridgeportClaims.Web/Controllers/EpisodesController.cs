using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
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

		public EpisodesController(IEpisodesDataProvider episodesDataProvider)
		{
			_episodesDataProvider = episodesDataProvider;
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
					var user = User.Identity.GetUserId();
					if (null == user)
						throw new ArgumentNullException(nameof(user));
					_episodesDataProvider.AddOrUpdateEpisode(model.EpisodeId, model.ClaimId, user, model.NoteText,
						model.EpisodeTypeId);
					return Ok(new { message = "Episode was saved Successfully" });
				});
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				return Content(HttpStatusCode.InternalServerError, new { message = ex.Message });
			}
		}

		[HttpGet]
		[Route("getepisodetypes")]
		public async Task<IHttpActionResult> GetEpisodeTypes()
		{
			try
			{
				return await Task.Run(() => Ok(_episodesDataProvider.GetEpisodeTypes()));
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				throw;
			}
		}
	}
}
