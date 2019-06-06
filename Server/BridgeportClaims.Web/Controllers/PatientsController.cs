using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.Patients;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/patients")]
    public class PatientsController : BaseApiController
    {
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);
        private readonly Lazy<IPatientProvider> _patientProvider;

        public PatientsController(Lazy<IPatientProvider> patientProvider)
        {
            _patientProvider = patientProvider;
        }


        [HttpPost]
        [Route("edit-patient")]
        public IHttpActionResult EditPatient(PatientEditModel model)
        {
            try
            {
                if (null == model)
                {
                    throw new ArgumentNullException(nameof(model));
                }
                var modifiedByUserId = User.Identity.GetUserId();
                _patientProvider.Value.UpdatePatientAddress(model.PatientId, modifiedByUserId, model.LastName, model.FirstName,
                    model.Address1, model.Address2, model.City, model.PostalCode, model.StateId, model.PhoneNumber, model.EmailAddress);
                return Ok(new { message = "The patient was updated successfully." });
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("get-patient-edit-report")]
        public IHttpActionResult GetPatientEditReport()
        {
            try
            {
                var data = _patientProvider.Value.GetPatientAddressReport();
                return Ok(data);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
