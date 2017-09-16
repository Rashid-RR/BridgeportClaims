﻿using NLog;
using System;
using System.Net;
using System.Web.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
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
                return Content(HttpStatusCode.InternalServerError, new { message = ex.Message });
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
                return Content(HttpStatusCode.InternalServerError, new { message = ex.Message });
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
                return Content(HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }

        #region Payment Posting Section

        [HttpPost]
        [Route("payment-posting")]
        public async Task<IHttpActionResult> PaymentPosting([FromBody] UserPaymentPostingSession model)
        {
            try
            {
                return await Task.Run(() =>
                {
                    if (null == model)
                        throw new ArgumentNullException(nameof(model));
                    if (null != model.LastAmountRemaining && model.AmountRemaining <= 0)
                        throw new Exception(
                            $"The 'LastAmountRemaining' field cannot be zero or negative. You passed in {model.LastAmountRemaining}");
                    if (null == model.SessionId || !_memoryCacher.Contains(model.SessionId))
                    {
                        model.UserName = User.Identity.Name;
                        model.SessionId = model.CacheKey;
                        _memoryCacher.AddItem(model.CacheKey, model);
                        var vm = Mapper.Map<UserPaymentPostingSession, PaymentPostingViewModel>(model);
                        return Ok(vm);
                    }
                    else
                    {
                        var existingModel = _memoryCacher.GetItem(model.CacheKey, false) as UserPaymentPostingSession;
                        if (null == existingModel)
                            throw new CacheException(
                                $"Error, tried to retrieve an object from cache that isn't there. Cache key {model.CacheKey}");
                        existingModel.UserName = User.Identity.Name;
                        // Make sure that the Prescription ID(s) do not already exist within the cached object.
                        var prescriptionIdExists = (from e in existingModel.PaymentPostings
                                    join m in model.PaymentPostings on e.PrescriptionId equals m.PrescriptionId
                                    select e).Any();
                        if (prescriptionIdExists)
                            throw new Exception("Error. There are one or more Prescription Id's passed in that already exist.");
                        existingModel.CheckNumber = model.CheckNumber;
                        existingModel.CheckAmount = model.CheckAmount;
                        existingModel.AmountSelected = model.AmountSelected;
                        existingModel.PaymentPostings.AddRange(model.PaymentPostings);
                        existingModel.UserName = User.Identity.Name;
                        _memoryCacher.UpdateItem(model.CacheKey, model);
                        var vm = Mapper.Map<UserPaymentPostingSession, PaymentPostingViewModel>(existingModel);
                        return Ok(vm);
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.InternalServerError, new { message = ex.Message });
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
                return Content(HttpStatusCode.InternalServerError, new {message = ex.Message});
            }
        }

        #endregion

    }
}