using NLog;
using System;
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
        private readonly IClaimNotesDataProvider _claimNotesDataProvider;
        private readonly IRepository<ClaimNote> _claimNoteRepository;

        public ClaimNotesController(IClaimNotesDataProvider claimNotesDataProvider, 
            IRepository<ClaimNote> claimNoteRepository)
        {
            _claimNotesDataProvider = claimNotesDataProvider;
            _claimNoteRepository = claimNoteRepository;
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
        [Route("{claimId:int}", Name = c.GetClaimNoteAction)]
        public async Task<IHttpActionResult> GetClaimNote(int claimId)
        {
            try
            {
                ClaimNote claimNote = null;
                await Task.Run(() =>
                {
                    claimNote = _claimNoteRepository.GetSingleOrDefault(x => x.Claim.ClaimId == claimId);
                });
                if (null != claimNote)
                    return Ok(claimNote);
                return NotFound();
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
                    var locationHeader = new Uri(Url.Link(c.GetClaimNoteAction, new { claimId }));
                    _claimNotesDataProvider.AddOrUpdateNote(claimId, noteText, User.Identity.GetUserId(), noteTypeId);
                    return Created(locationHeader, new { message = "The Claim Note was Saved Successfully"});
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
