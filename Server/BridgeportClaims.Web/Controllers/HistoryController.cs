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
        private readonly IClaimsUserHistoryProvider _claimsUserHistoryProvider;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public HistoryController(IClaimsUserHistoryProvider claimsUserHistoryProvider)
        {
            _claimsUserHistoryProvider = claimsUserHistoryProvider;
        }

        [HttpPost]
        [Route("addclaim")]
        public async Task<IHttpActionResult> AddClaimHistoryItem(int claimId)
        {
            try
            {
                return await Task.Run(() =>
                {
                    _claimsUserHistoryProvider.InsertClaimsUserHistory(User.Identity.GetUserId(), claimId);
                    return Ok(new {message = "Claim History Item Added Successfully"});
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("claims")]
        public async Task<IHttpActionResult> GetClaimHistory()
        {
            try
            {
                return await Task.Run(() => Ok(
                    _claimsUserHistoryProvider.GetClaimsUserHistory(User.Identity.GetUserId())));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
