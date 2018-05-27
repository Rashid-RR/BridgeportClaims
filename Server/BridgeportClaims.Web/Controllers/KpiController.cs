using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.KPI;
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
