using NLog;
using System;
using System.Collections.Generic;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.PrescriptionNoteTypes;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Entities.DomainModels;
using Microsoft.AspNet.Identity;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/prescriptionnotes")]
    public class PrescriptionNotesController : BaseApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IPrescriptionNoteTypesDataProvider _prescriptionNoteTypesDataProvider;
        private readonly IRepository<PrescriptionNote> _prescriptionNoteRepository;
        private readonly IRepository<AspNetUsers> _aspNetUsersRepository;

        public PrescriptionNotesController(IPrescriptionNoteTypesDataProvider prescriptionNoteTypesDataProvider,
                IRepository<PrescriptionNote> prescriptionNoteRepository, IRepository<AspNetUsers> aspNetUsersRepository)
        {
            _prescriptionNoteTypesDataProvider = prescriptionNoteTypesDataProvider;
            _prescriptionNoteRepository = prescriptionNoteRepository;
            _aspNetUsersRepository = aspNetUsersRepository;
        }

        [HttpPut]
        [Route("savenote")]
        public void AddOrUpdatePrescriptionNote(IList<int> prescriptionIds, string noteText, 
            int noteTypeId, int? prescriptionNoteId = null)
        {
            try
            {
                var now = DateTime.Now;
                PrescriptionNote note = null;
                if (null != prescriptionNoteId) // Update
                {
                    note = _prescriptionNoteRepository.Get(prescriptionNoteId.Value);
                    if (null == note)
                        throw new Exception($"Could not find a Prescription Note with Id: {prescriptionNoteId.Value}");
                    note.UpdatedOn = now;
                    note.NoteText = noteText;
                    note.PrescriptionNoteTypeId = noteTypeId;
                    note.AspNetUsers = _aspNetUsersRepository.Get(User.Identity.GetUserId());

                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpGet]
        [Route("notetypes")]
        public IList<PrescriptionNoteTypesDto> GetPrescriptionNoteTypes()
        {
            try
            {
                return _prescriptionNoteTypesDataProvider.GetPrescriptionNoteTypes();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}