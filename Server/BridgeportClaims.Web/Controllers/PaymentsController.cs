using NLog;
using System;
using System.Net;
using System.Web.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using AutoMapper;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Web.Models;
using BridgeportClaims.Business.Payments;
using BridgeportClaims.Common.Caching;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.Payments;
using NHibernate.Cache;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/payment")]
    public class PaymentsController : BaseApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IPaymentsDataProvider _paymentsDataProvider;
        private readonly IPaymentsBusiness _paymentsBusiness;
        private readonly IMemoryCacher _memoryCacher;

        public PaymentsController(
            IPaymentsDataProvider paymentsDataProvider, 
            IPaymentsBusiness paymentsBusiness, 
            IMemoryCacher memoryCacher)
        {
            _paymentsDataProvider = paymentsDataProvider;
            _paymentsBusiness = paymentsBusiness;
            _memoryCacher = memoryCacher;
        }

        [HttpPost]
        [Route("amount-remaining")]
        public async Task<IHttpActionResult> GetAmountRemaining(AmountRemainingModel model)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var retVal = _paymentsDataProvider.GetAmountRemaining(model.ClaimIds, model.CheckNumber);
                    return Ok(retVal);
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("payments-blade")]
        public async Task<IHttpActionResult> GetPrescriptionPayments(int claimId, string sort, string sortDirection, int page,
            int pageSize, string secondSort = null, string secondSortDirection = null)
        {
            try
            {
                return await Task.Run(() => Ok(
                    _paymentsDataProvider.GetPrescriptionPaymentsDtos(claimId, sort, sortDirection, page, pageSize,
                        secondSort, secondSortDirection)));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
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
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        /// <summary>
        /// First call, gets the intial Claims, to then drill into to enter the Prescrtions.
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
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
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

                    // Go down 2 paths: Payments Data Provider for a Full Payment or
                    var postPaymentReturnDto = _paymentsDataProvider.PostPayment(model.PrescriptionIds, model.CheckNumber,
                        model.CheckAmount, model.AmountSelected, model.AmountToPost);
                    var retVal = new PostPaymentReturnDto
                    {
                        ToastMessage = $"Payment posted successfully for {numberOfPrescriptions.Value} prescription" +
                                       $"{(1 == numberOfPrescriptions ? string.Empty : "s")}",
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
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        #region Payment Posting Section

        [HttpPost]
        [Route("to-suspense")]
        public async Task<IHttpActionResult> ToSuspense(SuspenseViewModel model)
        {
            try
            {
                return await Task.Run(() =>
                {
                    if (null == model)
                        throw new ArgumentNullException(nameof(model));
                    if (0 == model.ClaimId)
                        throw new Exception($"Error. Zero or NULL is not a valid Claim Id.");
                    string msg;
                    var cultureFormattedSuspenseAmount =
                        model.AmountToSuspense.ToString("C", new CultureInfo("en-US"));
                    if (model.SessionId.IsNotNullOrWhiteSpace() && _memoryCacher.Contains(model.SessionId))
                    {
                        // Now we are removing this item from memory.
                        var existingModel = _memoryCacher.GetItem(model.SessionId, true) as UserPaymentPostingSession;
                        if (null == existingModel)
                            throw new Exception("Error. The model retrieved from cache is null.");
                        // TODO: pass the model values and the session
                        msg = $"{existingModel.PaymentPostings.Count} payment{(existingModel.PaymentPostings.Count > 1 ? "s" : string.Empty)}" +
                              $" posted, and {cultureFormattedSuspenseAmount} to suspense have been saved successfully.";
                    }
                    else
                        msg = $"The amount of {cultureFormattedSuspenseAmount} has been saved to suspense successfully.";
                    return Ok(new { message = msg });
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("finalize-posting")]
        public async Task<IHttpActionResult> FinalizePosting(string sessionId)
        {
            try
            {
                return await Task.Run(() =>
                {
                    if (sessionId.IsNullOrWhiteSpace())
                        throw new Exception("Error. The sessionId parameter passed in cannot be null or empty.");
                    if (!_memoryCacher.Contains(sessionId))
                        throw new Exception($"Error. The Session Id {sessionId} cannot be found in memory.");
                    var model = _memoryCacher.GetItem(sessionId, false) as UserPaymentPostingSession;
                    if (null == model)
                        throw new Exception($"Error. The model cannot be found from Session Id: {sessionId}");
                    if (null == model.PaymentPostings?.Count || model.PaymentPostings.Count <= 0)
                        throw new Exception($"Error. There are no payment postings associated with this session (Id {sessionId}).");
                    // TODO: call the database, now with this model.
                    var num = model.PaymentPostings.Count;
                    var plural = num > 1 ? "s" : string.Empty;
                    return Ok(new {message = $"The {num} payment{plural} posted successfully."});
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        [HttpPost]
        [Route("get-existing-posted-payments")]
        public async Task<IHttpActionResult> GetExistingPostedPayments(string checkNumber, int claimId)
        {
            try
            {
                return await Task.Run(() =>
                {
                    // TODO: Pull this from the database
                    var ran = new Random();
                    var retVal = ran.Next(1, 8);
                    return Ok(new {message = retVal});
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("payment-posting")]
        public async Task<IHttpActionResult> PaymentPosting([FromBody] UserPaymentPostingSession model)
        {
            try
            {
                if (null == model)
                    throw new ArgumentNullException(nameof(model));
                if (null == model.SessionId || !_memoryCacher.Contains(model.SessionId))
                {
                    model.UserName = User.Identity.Name;
                    model.SessionId = model.CacheKey;
                    _memoryCacher.AddItem(model.CacheKey, model);
                    return await ReturnMappedViewModel(model);
                }
                var existingModel = _memoryCacher.GetItem(model.CacheKey, false) as UserPaymentPostingSession;
                if (null == existingModel)
                    throw new CacheException(
                        $"Error, tried to retrieve an object from cache that isn't there. Cache key {model.CacheKey}");
                existingModel.UserName = User.Identity.Name;
                // Make sure that the Prescription ID(s) do not already exist within the cached object.
                var prescriptionIdExists = existingModel.PaymentPostings.Join(model.PaymentPostings,
                    e => e.PrescriptionId, m => m.PrescriptionId, (e, m) => e).Any();
                if (prescriptionIdExists)
                    throw new Exception(
                        "Error. There are one or more Prescription Id's passed in that already exist.");
                existingModel.CheckNumber = model.CheckNumber;
                existingModel.CheckAmount = model.CheckAmount;
                existingModel.AmountSelected = model.AmountSelected;
                existingModel.PaymentPostings.AddRange(model.PaymentPostings);
                existingModel.UserName = User.Identity.Name;
                _memoryCacher.UpdateItem(model.CacheKey, model);
                return await ReturnMappedViewModel(existingModel);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        private async Task<IHttpActionResult> ReturnMappedViewModel(UserPaymentPostingSession model)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var vm = Mapper.Map<UserPaymentPostingSession, PaymentPostingViewModel>(model);
                    return Ok(vm);
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("cancel-posting")]
        public async Task<IHttpActionResult> CancelPosting(string sessionId)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var methodName = MethodBase.GetCurrentMethod().Name;
                    if (!_memoryCacher.Contains(sessionId))
                        Logger.Info($"The {methodName} method was called with no objects stored into session from ID {sessionId}.");
                    _memoryCacher.DeleteIfExists(sessionId);
                    return Ok(new {message = "The posted payments stored into cache have been cleared successfully."});
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("delete-posting")]
        public async Task<IHttpActionResult> DeletePosting(string sessionId, int prescriptionId)
        {
            try
            {
                return await Task.Run(() =>
                {
                    if (sessionId.IsNullOrWhiteSpace())
                        throw new ArgumentNullException(nameof(sessionId));
                    if (prescriptionId <= 0)
                        throw new Exception($"Error. The prescription Id {prescriptionId} is invalid.");
                    var existingModel = _memoryCacher.GetItem(sessionId, false) as UserPaymentPostingSession;
                    if (null == existingModel)
                        throw new Exception("Error. The existing model could not be retrieved from cache.");
                    existingModel.PaymentPostings.RemoveAll(x => x.PrescriptionId == prescriptionId);
                    _memoryCacher.UpdateItem(sessionId, existingModel);
                    return Ok(new { message = "The payment posting record was removed successfully." });
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        #endregion
    }
}