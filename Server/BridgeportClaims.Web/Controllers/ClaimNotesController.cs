using NLog;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.ClaimNotes;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Entities.DomainModels;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;
using c = BridgeportClaims.Common.StringConstants.Constants;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/claimnotes")]
    public class ClaimNotesController : BaseApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IRepository<Claim> _claimRepository;
        private readonly IClaimNotesDataProvider _claimNotesDataProvider;
        private readonly IRepository<ClaimNote> _claimNoteRepository;

        public ClaimNotesController(IClaimNotesDataProvider claimNotesDataProvider,
            IRepository<ClaimNote> claimNoteRepository, IRepository<Claim> claimRepository)
        {
            _claimNotesDataProvider = claimNotesDataProvider;
            _claimNoteRepository = claimNoteRepository;
            _claimRepository = claimRepository;
        }

        [HttpGet]
        [Route("notetypes")]
        public async Task<IHttpActionResult> GetClaimNoteType()
        {
            try
            {
                return await Task.Run(()
                    => Ok(_claimNotesDataProvider.GetClaimNoteTypes())).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }

        }

        [HttpPost]
        [Route("{claimId:int}", Name = c.GetClaimNoteAction)]
        public async Task<IHttpActionResult> GetClaimNote(int claimId)
        {
            try
            {
                ClaimNote claimNote = null;
                await Task.Run(() =>
                {
                    claimNote = _claimNoteRepository.GetSingleOrDefault(x => x.Claim.ClaimId == claimId);
                }).ConfigureAwait(false);
                if (null != claimNote)
                    return Ok(claimNote);
                return NotFound();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IHttpActionResult> DeleteClaimNote(int claimId)
        {
            try
            {
                return await Task.Run(() =>
                {
                    _claimNotesDataProvider.DeleteClaimNote(claimId);
                    return Ok(new { message = "The claim not was deleted successfully." });
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("savenote")]
        public async Task<IHttpActionResult> SaveNote(SaveClaimNoteModel model)
        {
            try
            {
                return await Task.Run(() =>
                {
                    if (null == model)
                        throw new ArgumentNullException(nameof(model));
                    var userId = User.Identity.GetUserId();
                    if (null == userId)
                        throw new ArgumentNullException(nameof(userId));
                    // validate that the Claim exists
                    if (null == _claimRepository.Get(model.ClaimId))
                        throw new Exception($"An error has occurred, claim Id {model.ClaimId} doesn't exist");
                    _claimNotesDataProvider.AddOrUpdateNote(model.ClaimId, model.NoteText, userId, model.NoteTypeId);
                    return Ok(new { message = "The Claim Note was Saved Successfully" });
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
