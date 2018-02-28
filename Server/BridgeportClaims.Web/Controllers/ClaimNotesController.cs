using NLog;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.ClaimNotes;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Entities.DomainModels;
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

        [HttpPost]
        [Route("savenote")]
        public async Task<IHttpActionResult> SaveNote(int claimId, string noteText, int? noteTypeId)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var userId = User.Identity.GetUserId();
                    if (null == userId)
                        throw new ArgumentNullException(nameof(userId));
                    // validate that the Claim exists
                    if (null == _claimRepository.Get(claimId))
                        throw new Exception($"An error has occurred, claim Id {claimId} doesn't exist");
                    _claimNotesDataProvider.AddOrUpdateNote(claimId, noteText, userId, noteTypeId);
                    return Ok(new {message = "The Claim Note was Saved Successfully"});
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }
    }
}
