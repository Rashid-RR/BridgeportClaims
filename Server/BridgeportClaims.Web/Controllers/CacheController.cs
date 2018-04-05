﻿using NLog;
using System;
using System.Net;
using System.Web.Http;
using System.Threading.Tasks;
using BridgeportClaims.Common.Caching;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/cache")]
    public class CacheController : BaseApiController
    {
        private static readonly Lazy<Logger> Logger = new Lazy<Logger>(LogManager.GetCurrentClassLogger);
        private readonly IMemoryCacher _cache;

        public CacheController()
        {
            _cache = MemoryCacher.Instance;
        }

        [HttpPost]
        [Route("clear")]
        public async Task<IHttpActionResult> ClearCache()
        {
            try
            {
                return await Task.Run(() =>
                {
                    _cache.DeleteAll();
                    return Ok(new {message = "Cache cleared successfully."});
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
