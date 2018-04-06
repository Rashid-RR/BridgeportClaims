﻿using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.AdjustorSearches;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/adjustors")]
    public class AdjustorsController : BaseApiController
    {
        private readonly Lazy<IAdjustorSearchProvider> _adjustorSearchProvider;
        private static readonly Lazy<Logger> Logger = new Lazy<Logger>(LogManager.GetCurrentClassLogger);

        public AdjustorsController(Lazy<IAdjustorSearchProvider> adjustorSearchProvider)
        {
            _adjustorSearchProvider = adjustorSearchProvider;
        }

        [HttpPost]
        [Route("search")]
        public IHttpActionResult GetAdjustorSearchResults(string searchText)
        {
            try
            {
                var results = _adjustorSearchProvider.Value.GetAdjustorSearchResults(searchText);
                return Ok(results);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
