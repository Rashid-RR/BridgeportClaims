using NLog;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
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
        [Route("getprescriptionnotes")]
        public async Task<IHttpActionResult> GetPrescriptionNotesByPrescriptionId(int prescriptionId)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var notes = _prescriptionNotesDataProvider.GetPrescriptionNotesByPrescriptionId(prescriptionId);
                    return Ok(notes);
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
        public async Task<IHttpActionResult> AddOrUpdatePrescriptionNote(PrescriptionNoteSaveDto dto)
        {
            try
            {
                return await Task.Run(() =>
                {
                    _prescriptionNotesDataProvider.AddOrUpdatePrescriptionNote(
                        dto, User.Identity.GetUserId());
                    return Ok(new {message = "The Prescription Note was Saved Successfully"});
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        

        [HttpGet]
        [Route("notetypes")]
        public async Task<IHttpActionResult> GetPrescriptionNoteTypes()
        {
            try
            {
                return await Task.Run(() =>
                {
                    var types = _prescriptionNoteTypesDataProvider.GetPrescriptionNoteTypes();
                    return Ok(types);
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}