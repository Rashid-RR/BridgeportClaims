using System;
using System.Net;
using System.Threading.Tasks;
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
        [Route("revenue")]
        public async Task<IHttpActionResult> GetMonthlyRevenue()
        {
            try
            {
                return await Task.Run(() =>
                {
                    var results = _kpiProvider.Value.GetPaymentTotalsDtos();
                    return Ok(results);
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }
    }
}
