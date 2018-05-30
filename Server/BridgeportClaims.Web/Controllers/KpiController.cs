using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.KPI;
using BridgeportClaims.Web.Models;
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

        [HttpPost]
        [Route("save-claim-merge")]
        public IHttpActionResult SaveClaimsMerge(MergeClaimModel model)
        {
            if (null == model)
            {
                throw new ArgumentNullException(nameof(model));
            }
            try
            {
                var results = _kpiProvider.Value.SaveClaimMerge(model.ClaimId, model.DuplicateClaimId,
                    model.ClaimNumber, model.PatientId
                    , model.InjuryDate, model.AdjustorId, model.PayorId, model.ClaimFlex2Id, model.PersonCode);
                return Ok(results);
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
