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
        private readonly IKpiProvider _kpiProvider;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public KpiController(IKpiProvider kpiProvider)
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
                    var results = _kpiProvider.GetPaymentTotalsDtos();
                    return Ok(results);
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }
    }
}
