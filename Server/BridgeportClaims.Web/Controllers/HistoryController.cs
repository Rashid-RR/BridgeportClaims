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
        private static readonly Lazy<Logger> Logger = new Lazy<Logger>(LogManager.GetCurrentClassLogger);

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
                return Ok(_claimsUserHistoryProvider.Value.GetClaimsUserHistory(User.Identity.GetUserId()));
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
