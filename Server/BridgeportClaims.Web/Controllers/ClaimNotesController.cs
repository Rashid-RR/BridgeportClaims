using System;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders;
using Microsoft.AspNet.Identity;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/claimnotes")]
    public class ClaimNotesController : BaseApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IClaimNotesDataProvider _claimNotesDataProvider;

        public ClaimNotesController(IClaimNotesDataProvider claimNotesDataProvider)
        {
            _claimNotesDataProvider = claimNotesDataProvider;
        }

        [HttpPost]
        [Route("savenote")]
        public async Task<IHttpActionResult>SaveNote(int claimId, string noteText, int noteTypeId)
        {
            try
            {
                return await Task.Run(() =>
                {
                    _claimNotesDataProvider.AddOrUpdateNote(claimId, noteText, User.Identity.GetUserId(), noteTypeId);
                    return Ok(new { message = "The Claim Note was Saved Successfully"});
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
