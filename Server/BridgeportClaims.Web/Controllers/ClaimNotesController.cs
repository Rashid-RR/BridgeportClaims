using NLog;
using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.ClaimNotes;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/claimnotes")]
    public class ClaimNotesController : BaseApiController
    {
        private static readonly Lazy<Logger> Logger = new Lazy<Logger>(LogManager.GetCurrentClassLogger);
        private readonly Lazy<IClaimNotesDataProvider> _claimNotesDataProvider;

        public ClaimNotesController(Lazy<IClaimNotesDataProvider> claimNotesDataProvider)
        {
            _claimNotesDataProvider = claimNotesDataProvider;
        }

        [HttpGet]
        [Route("notetypes")]
        public IHttpActionResult GetClaimNoteType()
        {
            try
            {
                return Ok(_claimNotesDataProvider.Value.GetClaimNoteTypes());
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpDelete]
        [Route("delete")]
        public IHttpActionResult DeleteClaimNote(int claimId)
        {
            try
            {
                _claimNotesDataProvider.Value.DeleteClaimNote(claimId);
                return Ok(new {message = "The claim not was deleted successfully."});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        [HttpPost]
        [Route("savenote")]
        public IHttpActionResult SaveNote(SaveClaimNoteModel model)
        {
            try
            {
                if (null == model)
                    throw new ArgumentNullException(nameof(model));
                var userId = User.Identity.GetUserId();
                if (null == userId)
                    throw new ArgumentNullException(nameof(userId));
                _claimNotesDataProvider.Value.AddOrUpdateNote(model.ClaimId, model.NoteText, userId, model.NoteTypeId);
                return Ok(new {message = "The Claim Note was Saved Successfully"});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }
    }
}
