using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.Payments;
using BridgeportClaims.Web.Models;
using System.Net;
using BridgeportClaims.Business.Payments;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/payment")]
    public class PaymentsController : BaseApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IPaymentsDataProvider _paymentsDataProvider;
        private readonly IPaymentsBusiness _paymentsBusiness;

        public PaymentsController(IPaymentsDataProvider paymentsDataProvider, IPaymentsBusiness paymentsBusiness)
        {
            _paymentsDataProvider = paymentsDataProvider;
            _paymentsBusiness = paymentsBusiness;
        }

        [HttpPost]
        [Route("claims-script-details")]
        public async Task<IHttpActionResult> GetClaimsWithPrescriptionDetails(IList<int> claimIds)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var claimsWithScripts = _paymentsDataProvider.GetClaimsWithPrescriptionDetails(claimIds);
                    return Ok(claimsWithScripts);
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }

        /// <summary>
        /// First call, gets the intial Claims, to then drill into to enter the Prescriios d
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("claims-script-counts")]
        public async Task<IHttpActionResult> GetClaimsWithPrescriptionCounts([FromBody] PaymentsSearchModel model)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var payments = _paymentsDataProvider.GetClaimsWithPrescriptionCounts(model.ClaimNumber,
                        model.FirstName, model.LastName, model.RxDate, model.InvoiceNumber);
                    return Ok(payments);
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("post-payments")]
        public async Task<IHttpActionResult> PostPayments(PostPaymentsModel model)
        {
            try
            {
                return await Task.Run(() =>
                {
                    if (null == model)
                        throw new ArgumentNullException(nameof(model));
                    var numberOfPrescriptions = model.PrescriptionIds?.Count;
                    if (null == numberOfPrescriptions || numberOfPrescriptions < 1)
                        throw new Exception("Error. There were no Prescriptions sent.");
                    if (!ModelState.IsValid)
                        throw new Exception("Error. The inputted fields are not in the correct format. " +
                                            "Model state is not valid.");
                    // Business rule check.
                    if (!_paymentsBusiness.CheckMultiLinePartialPayments(model.AmountSelected, model.AmountToPost,
                        numberOfPrescriptions.Value))
                        throw new Exception(
                            "Error. Multi-prescription, partial payments are not supported at this time.");

                    var postPaymentReturnDto = _paymentsDataProvider.PostPayment(model.PrescriptionIds, model.CheckNumber,
                        model.CheckAmount, model.AmountSelected,
                        model.AmountToPost);

                    // construct, stubbed return model. TODO: replace stubbed data with real data from the proc.
                    var retVal = new PostPaymentReturnDto
                    {
                        ToastMessage = "Payment posted successfully " + Environment.NewLine +
                                       $"for {numberOfPrescriptions.Value} prescription{(1 == numberOfPrescriptions ? string.Empty : "s")}",
                        AmountRemaining = postPaymentReturnDto.AmountRemaining
                    };
                    foreach (var prescription in postPaymentReturnDto.PostPaymentPrescriptionReturnDtos)
                        retVal.PostPaymentPrescriptionReturnDtos.Add(new PostPaymentPrescriptionReturnDto
                        {
                            PrescriptionId = prescription.PrescriptionId,
                            Outstanding = prescription.Outstanding
                        });
                    return Ok(retVal);
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }
    }
}