using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders;
using Microsoft.AspNet.Identity;
using RazorEngine.Templating;

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

        [HttpGet]
        [Route("notetypes")]
        public async Task<IHttpActionResult> GetClaimNoteType()
        {
            try
            {
                return await Task.Run(() 
                    => Ok(_claimNotesDataProvider.GetClaimNoteTypes()));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }

        }

        [HttpPost]
        [Route("savenote")]
        public async Task<IHttpActionResult> SaveNote(int claimId, string noteText, int noteTypeId)
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
