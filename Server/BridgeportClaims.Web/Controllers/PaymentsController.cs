﻿using NLog;
using System;
using System.Net;
using System.Web.Http;
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
using Microsoft.AspNet.Identity;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/payment")]
    public class PaymentsController : BaseApiController
    {
        private static readonly Lazy<Logger> Logger = new Lazy<Logger>(LogManager.GetCurrentClassLogger);
        private readonly Lazy<IPaymentsDataProvider> _paymentsDataProvider;
        private readonly Lazy<IPaymentsBusiness> _paymentsBusiness;
        private readonly Lazy<IMemoryCacher> _memoryCacher;
        private static readonly UserPaymentPostingSession Shell = null;

        public PaymentsController(
            Lazy<IPaymentsDataProvider> paymentsDataProvider, 
            Lazy<IPaymentsBusiness> paymentsBusiness)
        {
            _paymentsDataProvider = paymentsDataProvider;
            _paymentsBusiness = paymentsBusiness;
            _memoryCacher = new Lazy<IMemoryCacher>(() => MemoryCacher.Instance);
        }

        [HttpPost]
        [Route("payments-blade")]
        public IHttpActionResult GetPrescriptionPayments(int claimId, string sort, string sortDirection, int page,
            int pageSize, string secondSort = null, string secondSortDirection = null)
        {
            try
            {
                return Ok(_paymentsDataProvider.Value.GetPrescriptionPaymentsDtos(claimId, sort, sortDirection, page, pageSize, secondSort, secondSortDirection));
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        [HttpPost]
        [Route("claims-script-details")]
        public IHttpActionResult GetClaimsWithPrescriptionDetails(IList<int> claimIds)
        {
            try
            {
                var claimsWithScripts = _paymentsDataProvider.Value.GetClaimsWithPrescriptionDetails(claimIds);
                return Ok(claimsWithScripts);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        /// <summary>
        /// First call, gets the initial Claims, to then drill into to enter the Prescrtions.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("claims-script-counts")]
        public IHttpActionResult GetClaimsWithPrescriptionCounts([FromBody] PaymentsSearchModel model)
        {
            try
            {
                var payments = _paymentsDataProvider.Value.GetClaimsWithPrescriptionCounts(model.ClaimNumber,
                    model.FirstName, model.LastName, model.RxDate.ToNullableFormattedDateTime(), model.InvoiceNumber);
                return Ok(payments);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        [HttpPost]
        [Route("post-payments")]
        public IHttpActionResult PostPayments(PostPaymentsModel model)
        {
            try
            {
                if (null == model)
                {
                    throw new ArgumentNullException(nameof(model));
                }
                var numberOfPrescriptions = model.PrescriptionIds?.Count;
                if (null == numberOfPrescriptions || numberOfPrescriptions < 1)
                {
                    throw new Exception("Error. There were no Prescriptions sent.");
                }
                if (!ModelState.IsValid)
                {
                    throw new Exception("Error. The inputted fields are not in the correct format. " +
                                        "Model state is not valid.");
                }
                // Business rule check.
                if (!_paymentsBusiness.Value.CheckMultiLinePartialPayments(model.AmountSelected, model.AmountToPost,
                    numberOfPrescriptions.Value))
                {
                    throw new Exception(
                        "Error. Multi-prescription, partial payments are not supported at this time.");
                }
                // Go down 2 paths: Payments Data Provider for a Full Payment or
                var postPaymentReturnDto = _paymentsDataProvider.Value.PostPayment(model.PrescriptionIds,
                    model.CheckNumber, model.CheckAmount, model.AmountSelected, model.AmountToPost, model.DocumentId);
                postPaymentReturnDto.ToastMessage =
                    $"Payment posted successfully for {numberOfPrescriptions.Value} prescription" +
                    $"{(1 == numberOfPrescriptions ? string.Empty : "s")}";
                postPaymentReturnDto.AmountRemaining = postPaymentReturnDto.AmountRemaining;
                return Ok(postPaymentReturnDto);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        /*[HttpPost]
        [Route("post-payments")]
        public IHttpActionResult PostPayments(PostPaymentsModel model)
        {
            try
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
                if (!_paymentsBusiness.Value.CheckMultiLinePartialPayments(model.AmountSelected, model.AmountToPost,
                    numberOfPrescriptions.Value))
                    throw new Exception(
                        "Error. Multi-prescription, partial payments are not supported at this time.");

                // Go down 2 paths: Payments Data Provider for a Full Payment or
                var postPaymentReturnDto = _paymentsDataProvider.Value.PostPayment(model.PrescriptionIds,
                    model.CheckNumber, model.CheckAmount, model.AmountSelected, model.AmountToPost, model.DocumentId);
                var retVal = new PostPaymentReturnDto
                {
                    ToastMessage = $"Payment posted successfully for {numberOfPrescriptions.Value} prescription" +
                                   $"{(1 == numberOfPrescriptions ? string.Empty : "s")}",
                    AmountRemaining = postPaymentReturnDto.AmountRemaining
                };
                var checkDto = _paymentsDataProvider.Value.GetCheckDto(model.DocumentId);
                foreach (var prescription in postPaymentReturnDto.PostPaymentPrescriptionReturnDtos)
                {
                    retVal.PostPaymentPrescriptionReturnDtos.Add(new PostPaymentPrescriptionReturnDto
                    {
                        PrescriptionId = prescription.PrescriptionId,
                        Outstanding = prescription.Outstanding,
                    });
                }
                retVal.DocumentId = model.DocumentId;
                retVal.FileName = checkDto.FileName;
                retVal.FileUrl = checkDto.FileUrl;
                return Ok(retVal);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }*/

        #region New Payment Posting Section

        [HttpPost]
        [Route("to-suspense")]
        public IHttpActionResult ToSuspense(SuspenseViewModel model)
        {
            try
            {
                if (null == model)
                    throw new ArgumentNullException(nameof(model));
                if (model.AmountToSuspense == 0)
                    throw new Exception("Error, cannot add a zero or empty dollar amount to suspense.");
                if (model.CheckNumber.IsNullOrWhiteSpace())
                    throw new Exception("Error. Must provide a valid check number.");
                string msg;
                var cultureFormattedSuspenseAmount =
                    model.AmountToSuspense.ToString("C", new CultureInfo("en-US"));
                var userId = User.Identity.GetUserId();
                if (null == userId)
                    throw new Exception("Error. Could not find the authenticated user Id.");
                if (model.SessionId.IsNotNullOrWhiteSpace() && _memoryCacher.Value.Contains(model.SessionId))
                {
                    var existingModel = _memoryCacher.Value.AddOrGetExisting(model.SessionId, () => Shell);
                    if (null == existingModel)
                        throw new Exception("Error. The model retrieved from cache is null.");
                    existingModel.UserId = userId;
                    // Add the suspense items to the existing model.
                    existingModel.SuspenseAmountRemaining = model.AmountToSuspense;
                    existingModel.ToSuspenseNoteText = model.NoteText;
                    msg =
                        $"{existingModel.PaymentPostings.Count} payment{(existingModel.PaymentPostings.Count > 1 ? "s" : string.Empty)}" +
                        $" posted, and {cultureFormattedSuspenseAmount} to suspense have been saved successfully.";
                    // Database Call.
                    FinalizePaymentPostingDbCall(existingModel);
                    // Final Cleanup.
                    _memoryCacher.Value.Delete(model.SessionId);
                }
                else
                {
                    // Database call.
                    _paymentsDataProvider.Value.PrescriptionPostings(model.CheckNumber, true, model.AmountToSuspense,
                        model.NoteText, null, userId, null);
                    msg =
                        $"The amount of {cultureFormattedSuspenseAmount} has been saved to suspense successfully.";
                    // Nothing in memory so nothing to clean up.
                }

                return Ok(new {message = msg});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        private void FinalizePaymentPostingDbCall(UserPaymentPostingSession model)
        {
            var userId = User.Identity.GetUserId();
            if (userId.IsNullOrWhiteSpace())
                throw new Exception("Could not locate a User Id for the Logged In User.");
            var list = Mapper.Map<IList<PaymentPosting>, IList<PaymentPostingDto>>(model.PaymentPostings);
            _paymentsDataProvider.Value.PrescriptionPostings(model.CheckNumber, model.HasSuspense, model.SuspenseAmountRemaining,
                model.ToSuspenseNoteText, model.AmountsToPost, userId, list);
        }

        [HttpPost]
        [Route("finalize-posting")]
        public IHttpActionResult FinalizePosting(string sessionId)
        {
            try
            {
                if (sessionId.IsNullOrWhiteSpace())
                    throw new Exception("Error. The sessionId parameter passed in cannot be null or empty.");
                if (!_memoryCacher.Value.Contains(sessionId))
                    throw new Exception($"Error. The Session Id {sessionId} cannot be found in memory.");
                var model = _memoryCacher.Value.AddOrGetExisting(sessionId, () => Shell);
                if (null == model)
                    throw new Exception($"Error. The model cannot be found from Session Id: {sessionId}");
                if (null == model.PaymentPostings?.Count || model.PaymentPostings.Count <= 0)
                    throw new Exception(
                        $"Error. There are no payment postings associated with this session (Id {sessionId}).");
                // Database Call
                FinalizePaymentPostingDbCall(model);
                // Final Cleanup.
                _memoryCacher.Value.Delete(model.SessionId);
                var num = model.PaymentPostings.Count;
                var plural = num != 1 ? "s" : string.Empty;
                return Ok(new {message = $"The {num} payment{plural} posted successfully."});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }
        
        [HttpPost]
        [Route("payment-posting")]
        public IHttpActionResult PaymentPosting([FromBody] UserPaymentPostingSession model)
        {
            try
            {
                if (null == model)
                    throw new ArgumentNullException(nameof(model));
                if (null == model.SessionId || !_memoryCacher.Value.Contains(model.SessionId))
                {
                    model.SessionId = model.CacheKey;
                    _memoryCacher.Value.AddOrGetExisting(model.CacheKey, () => model);
                    return ReturnMappedViewModel(model);
                }
                var existingModel = _memoryCacher.Value.AddOrGetExisting(model.CacheKey, () => Shell);
                if (null == existingModel)
                {
                    throw new Exception(
                        $"Error, tried to retrieve an object from cache that isn't there. Cache key {model.CacheKey}");
                }
                existingModel.CheckNumber = model.CheckNumber;
                existingModel.CheckAmount = model.CheckAmount;
                existingModel.AmountSelected = model.AmountSelected;
                existingModel.PaymentPostings.AddRange(model.PaymentPostings);
                _memoryCacher.Value.UpdateItem(model.CacheKey, model);
                return ReturnMappedViewModel(existingModel);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        private IHttpActionResult ReturnMappedViewModel(UserPaymentPostingSession model)
        {
            try
            {
                var vm = Mapper.Map<UserPaymentPostingSession, PaymentPostingViewModel>(model);
                return Ok(vm);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        [HttpPost]
        [Route("cancel-posting")]
        public IHttpActionResult CancelPosting(string sessionId)
        {
            try
            {
                var methodName = MethodBase.GetCurrentMethod().Name;
                if (!_memoryCacher.Value.Contains(sessionId))
                    Logger.Value.Info(
                        $"The {methodName} method was called with no objects stored into session from ID {sessionId}.");
                _memoryCacher.Value.Delete(sessionId);
                return Ok(new {message = "The posted payments stored into cache have been cleared successfully."});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        [HttpPost]
        [Route("delete-posting")]
        public IHttpActionResult DeletePosting(string sessionId, int prescriptionId, int id)
        {
            try
            {
                if (sessionId.IsNullOrWhiteSpace())
                    throw new ArgumentNullException(nameof(sessionId));
                if (prescriptionId <= 0)
                    throw new Exception($"Error. The prescription Id {prescriptionId} is invalid.");
                var existingModel = _memoryCacher.Value.AddOrGetExisting(sessionId, () => Shell);
                if (null == existingModel)
                    throw new Exception("Error. The existing model could not be retrieved from cache.");
                var prescriptionIdPassedInExistsInCache =
                    existingModel.PaymentPostings.Any(x => x.PrescriptionId == prescriptionId);
                if (!prescriptionIdPassedInExistsInCache)
                    throw new Exception($"Error. The prescription Id {prescriptionId} doesn't exist in cache.");
                existingModel.PaymentPostings.RemoveAll(x => x.PrescriptionId == prescriptionId && x.Id == id);
                _memoryCacher.Value.UpdateItem(sessionId, existingModel);
                return Ok(new
                {
                    message = "The payment posting record was removed successfully.",
                    amountRemaining = existingModel.AmountRemaining
                });
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        #endregion
    }
}