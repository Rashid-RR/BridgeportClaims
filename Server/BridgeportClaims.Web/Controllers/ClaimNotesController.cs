using NLog;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.ClaimNotes;
using BridgeportClaims.RedisCache.Clearing;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/claimnotes")]
    public class ClaimNotesController : BaseApiController
    {
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);
        private readonly Lazy<IClaimNotesDataProvider> _claimNotesDataProvider;
        private readonly Lazy<ICachingClearingService> _cachingClearingService;

        public ClaimNotesController(Lazy<IClaimNotesDataProvider> claimNotesDataProvider,
            Lazy<ICachingClearingService> cachingClearingService)
        {
            _claimNotesDataProvider = claimNotesDataProvider;
            _cachingClearingService = cachingClearingService;
        }

        [HttpGet]
        [Route("notetypes")]
        public async Task<IHttpActionResult> GetClaimNoteType()
        {
            try
            {
                var results = await _claimNotesDataProvider.Value.GetClaimNoteTypesAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
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
