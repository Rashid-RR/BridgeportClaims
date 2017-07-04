using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Business.Models;
using BridgeportClaims.Data.DataProviders.PrescriptionNotes;
using BridgeportClaims.Data.DataProviders.PrescriptionNoteTypes;
using BridgeportClaims.Data.Dtos;
using Microsoft.AspNet.Identity;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/prescriptionnotes")]
    public class PrescriptionNotesController : BaseApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IPrescriptionNoteTypesDataProvider _prescriptionNoteTypesDataProvider;
        private readonly IPrescriptionNotesDataProvider _prescriptionNotesDataProvider;

        public PrescriptionNotesController(IPrescriptionNoteTypesDataProvider prescriptionNoteTypesDataProvider,
            IPrescriptionNotesDataProvider prescriptionNotesDataProvider)
        {
            _prescriptionNoteTypesDataProvider = prescriptionNoteTypesDataProvider;
            _prescriptionNotesDataProvider = prescriptionNotesDataProvider;
        }

        [HttpPost]
        [Route("savenote")]
        public async Task<IHttpActionResult> AddOrUpdatePrescriptionNote(PrescriptionNoteSaveModel model)
        {
            try
            {
                await _prescriptionNotesDataProvider.AddOrUpdatePrescriptionNoteAsync(
                    model, User.Identity.GetUserId());
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        

        [HttpGet]
        [Route("notetypes")]
        public async Task<IList<PrescriptionNoteTypesDto>> GetPrescriptionNoteTypes()
        {
            try
            {
                return await Task.Run(() => _prescriptionNoteTypesDataProvider.GetPrescriptionNoteTypes());
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}