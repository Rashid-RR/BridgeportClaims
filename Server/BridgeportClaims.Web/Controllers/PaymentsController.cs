using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.Payments;
using BridgeportClaims.Web.Models;
using System.Net;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/payment")]
    public class PaymentsController : BaseApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IPaymentsDataProvider _paymentsDataProvider;

        public PaymentsController(IPaymentsDataProvider paymentsDataProvider)
        {
            _paymentsDataProvider = paymentsDataProvider;
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
                return Content(HttpStatusCode.InternalServerError, new {message = ex.Message});
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
                return Content(HttpStatusCode.InternalServerError, new {message = ex.Message});
            }
        }

        [HttpPost]
        [Route("post-payments")]
        public async Task<IHttpActionResult> PostPayments(PostPaymentsModel model)
        {
            try
            {
                if (null == model)
                    throw new ArgumentNullException(nameof(model));
                if (!ModelState.IsValid)
                    throw new Exception("Error. The inputted fields are not in the correct format.");
                var numberOfPrescriptions = model.PrescriptionIds?.Count;
                switch (numberOfPrescriptions)
                {
                    case null:
                        throw new Exception("Error. There were no Prescriptions sent.");
                    case 0:
                        throw new Exception("Error. No prescriptions were sent.");
                }
                await Task.Run(() =>
                {
                    _paymentsDataProvider.PostPaymentAsync(model.PrescriptionIds, model.CheckNumber,
                        model.CheckAmount, model.AmountSelected,
                        model.AmountToPost);
                });
                return Ok(new {message = $"Payment{(1 == numberOfPrescriptions ? string.Empty : "s")} Posted Successfully!"});
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.InternalServerError, new {message = ex.Message});
            }
        }
    }
}