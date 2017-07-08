using System;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.Episodes;
using BridgeportClaims.Data.Dtos;
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
        public IHttpActionResult AddOrUpdateEpisode([FromBody] EpisodeDto episode)
        {
            try
            {
                if (!ModelState.IsValid)
                    return GetBadRequestFormattedErrorMessages();
                _episodesDataProvider.AddOrUpdateEpisode(episode);
                return Ok(new { message = "Episode was saved Successfully" });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return InternalServerError(ex);
            }
        }
    }
}
