using NLog;
using System;
using System.Net;
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
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);
        private readonly Lazy<IPrescriptionNoteTypesDataProvider> _prescriptionNoteTypesDataProvider;
        private readonly Lazy<IPrescriptionNotesDataProvider> _prescriptionNotesDataProvider;

        public PrescriptionNotesController(Lazy<IPrescriptionNoteTypesDataProvider> prescriptionNoteTypesDataProvider,
            Lazy<IPrescriptionNotesDataProvider> prescriptionNotesDataProvider)
        {
            _prescriptionNoteTypesDataProvider = prescriptionNoteTypesDataProvider;
            _prescriptionNotesDataProvider = prescriptionNotesDataProvider;
        }

        [HttpPost]
        [Route("getprescriptionnotes")]
        public IHttpActionResult GetPrescriptionNotesByPrescriptionId(int prescriptionId)
        {
            try
            {
                var notes = _prescriptionNotesDataProvider.Value.GetPrescriptionNotesByPrescriptionId(prescriptionId);
                return Ok(notes);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        [HttpPost]
        [Route("savenote")]
        public IHttpActionResult AddOrUpdatePrescriptionNote(PrescriptionNoteSaveDto dto)
        {
            try
            {
                _prescriptionNotesDataProvider.Value.AddOrUpdatePrescriptionNote(
                    dto, User.Identity.GetUserId());
                return Ok(new
                {
                    message = $"The {(dto.IsDiaryEntry ? "diary entry and " : string.Empty)}prescription note " +
                              $"{(!dto.IsDiaryEntry ? "was" : "were")} saved successfully"
                });
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        

        [HttpGet]
        [Route("notetypes")]
        public IHttpActionResult GetPrescriptionNoteTypes()
        {
            try
            {
                var types = _prescriptionNoteTypesDataProvider.Value.GetPrescriptionNoteTypes();
                return Ok(types);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }
    }
}