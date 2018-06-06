using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.KPI;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/kpi")]
    public class KpiController : BaseApiController
    {
        private readonly Lazy<IKpiProvider> _kpiProvider;
        private static readonly Lazy<Logger> Logger = new Lazy<Logger>(LogManager.GetCurrentClassLogger);

        public KpiController(Lazy<IKpiProvider> kpiProvider)
        {
            _kpiProvider = kpiProvider;
        }

        private void ValidateModel(MergeClaimModel model)
        {
            if (null == model)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (default(int) == model.ClaimId)
            {
                throw new ArgumentException($"{nameof(model.ClaimId)} cannot be zero.");
            }
            if (default(int) == model.DuplicateClaimId)
            {
                throw new ArgumentException($"{nameof(model.DuplicateClaimId)} cannot be zero.");
            }
            if (model.ClaimNumber.IsNullOrWhiteSpace())
            {
                throw new Exception($"{model.ClaimNumber} parameter cannot be empty.");
            }
            if (default(int) == model.PatientId)
            {
                throw new ArgumentException($"{nameof(model.PatientId)} cannot be zero.");
            }
            if (default(int) == model.PayorId)
            {
                throw new ArgumentException($"{nameof(model.PayorId)} cannot be zero.");
            }
        }

        [HttpPost]
        [Route("save-claim-merge")]
        public IHttpActionResult SaveClaimsMerge(MergeClaimModel model)
        {
            ValidateModel(model);
            try
            {
                var userId = User.Identity.GetUserId();
                var succeeded = _kpiProvider.Value.SaveClaimMerge(model.ClaimId, model.DuplicateClaimId, userId,
                    model.ClaimNumber, model.PatientId
                    , model.InjuryDate.ToNullableFormattedDateTime(), model.AdjustorId, model.PayorId, model.ClaimFlex2Id);
                if (!succeeded)
                {
                    throw new Exception($"Error, something happened that didn't allow the database call to succeed.");
                }
                return Ok(new {message = "The claims were merged successfully."});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("get-left-right-claims")]
        public IHttpActionResult GetClaimComparisons(int leftClaimId, int rightClaimId)
        {
            try
            {
                var results = _kpiProvider.Value.GetClaimComparisons(leftClaimId, rightClaimId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("revenue")]
        public IHttpActionResult GetMonthlyRevenue()
        {
            try
            {
                var results = _kpiProvider.Value.GetPaymentTotalsDtos();
                return Ok(results);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }
    }
}
