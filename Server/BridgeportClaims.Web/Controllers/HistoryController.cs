using System;
using System.Net;
using System.Threading.Tasks;
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
        public async Task<IHttpActionResult> AddClaimHistoryItem(int claimId)
        {
            try
            {
                await _claimsUserHistoryProvider.Value.InsertClaimsUserHistoryAsync(User.Identity.GetUserId(), claimId);
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
        public async Task<IHttpActionResult> GetClaimHistory()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var results = await _claimsUserHistoryProvider.Value.GetClaimsUserHistoryAsync(userId);
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
