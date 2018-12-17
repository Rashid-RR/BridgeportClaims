using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.ClaimsUserHistories;
using Microsoft.AspNet.Identity;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/history")]
    public class HistoryController : BaseApiController
    {
        private readonly Lazy<IClaimsUserHistoryProvider> _claimsUserHistoryProvider;
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);

        public HistoryController(Lazy<IClaimsUserHistoryProvider> claimsUserHistoryProvider)
        {
            _claimsUserHistoryProvider = claimsUserHistoryProvider;
        }

        [HttpPost]
        [Route("addclaim")]
        public IHttpActionResult AddClaimHistoryItem(int claimId)
        {
            try
            {
                _claimsUserHistoryProvider.Value.InsertClaimsUserHistory(User.Identity.GetUserId(), claimId);
                return Ok(new {message = "Claim History Item Added Successfully"});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        [HttpGet]
        [Route("claims")]
        public IHttpActionResult GetClaimHistory()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var results = _claimsUserHistoryProvider.Value.GetClaimsUserHistory(userId);
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
