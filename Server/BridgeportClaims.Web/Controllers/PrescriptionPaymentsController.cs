using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.PrescriptionPayments;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/prescription-payments")]
    public class PrescriptionPaymentsController : BaseApiController
    {
        private readonly Lazy<IPrescriptionPaymentProvider> _provider;
        private static readonly Lazy<Logger> Logger = new Lazy<Logger>(LogManager.GetCurrentClassLogger);

        public PrescriptionPaymentsController(Lazy<IPrescriptionPaymentProvider> provider)
        {
            _provider = provider;
        }

        [HttpPost]
        [Route("delete")]
        public IHttpActionResult DeletePrescriptionPayment(int prescriptionPaymentId)
        {
            try
            {
                _provider.Value.DeletePrescriptionPayment(prescriptionPaymentId);
                return Ok(new {message = "The prescription payment was deleted successfully."});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        [HttpPost]
        [Route("update")]
        public IHttpActionResult UpdatePrescriptionPayment(PrescriptionPaymentViewModel model)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                _provider.Value.UpdatePrescriptionPayment(model.PrescriptionPaymentId, model.CheckNumber,
                    model.AmountPaid,
                    model.DatePosted.ToNullableFormattedDateTime(), model.PrescriptionId, userId);
                return Ok(new {message = "The prescription payment was updated successfully."});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
