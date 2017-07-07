using System;
using System.Threading.Tasks;
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
        public async Task<IHttpActionResult> AddOrUpdateEpisode([FromBody] EpisodeDto episode)
        {
            try
            {
                return await Task.Run(() =>
                {
                    _episodesDataProvider.AddOrUpdateEpisode(episode);
                    return Ok(new {message = "Episode was saved Successfully"});
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
