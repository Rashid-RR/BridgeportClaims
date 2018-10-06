using BridgeportClaims.Common.Caching;
using NLog;
using System;
using System.Net;
using System.Web.Http;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/cache")]
    public class CacheController : BaseApiController
    {
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);
        private readonly Lazy<IMemoryCacher> _cache;

        public CacheController()
        {
            _cache = new Lazy<IMemoryCacher>(() => MemoryCacher.Instance);
        }

        [HttpPost]
        [Route("clear")]
        public IHttpActionResult ClearCache()
        {
            try
            {
                _cache.Value.DeleteAll();
                return Ok(new {message = "Cache cleared successfully."});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }
    }
}
