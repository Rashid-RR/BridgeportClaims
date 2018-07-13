using NLog;
using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Common.Constants;
using BridgeportClaims.Data.DataProviders.ClaimNotes;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Entities.DomainModels;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/claimnotes")]
    public class ClaimNotesController : BaseApiController
    {
        private static readonly Lazy<Logger> Logger = new Lazy<Logger>(LogManager.GetCurrentClassLogger);
        private readonly Lazy<IRepository<Claim>> _claimRepository;
        private readonly Lazy<IClaimNotesDataProvider> _claimNotesDataProvider;
        private readonly Lazy<IRepository<ClaimNote>> _claimNoteRepository;

        public ClaimNotesController(Lazy<IClaimNotesDataProvider> claimNotesDataProvider,
            Lazy<IRepository<ClaimNote>> claimNoteRepository, Lazy<IRepository<Claim>> claimRepository)
        {
            _claimNotesDataProvider = claimNotesDataProvider;
            _claimNoteRepository = claimNoteRepository;
            _claimRepository = claimRepository;
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

        [HttpPost]
        [Route("{claimId:int}", Name = StringConstants.GetClaimNoteAction)]
        public IHttpActionResult GetClaimNote(int claimId)
        {
            try
            {
                var claimNote = _claimNoteRepository.Value.GetSingleOrDefault(x => x.Claim.ClaimId == claimId);
                if (null != claimNote)
                    return Ok(claimNote);
                return NotFound();
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
                // validate that the Claim exists
                if (null == _claimRepository.Value.Get(model.ClaimId))
                    throw new Exception($"An error has occurred, claim Id {model.ClaimId} doesn't exist");
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
