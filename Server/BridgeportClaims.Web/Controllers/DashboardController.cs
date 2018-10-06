using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.Dashboards;
using cs= BridgeportClaims.Common.Config.ConfigService;

using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/dashboard")]
    public class DashboardController : BaseApiController
    {
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);
        private readonly Lazy<IDashboardProvider> _dashboardProvider;

        public DashboardController(Lazy<IDashboardProvider> dashboardProvider)
        {
            _dashboardProvider = dashboardProvider;
        }

        [HttpPost]
        [Route("kpi")]
        public IHttpActionResult GetDashboardKpis()
        {
            try
            {
                var isProduction = cs.IsProduction;
                return Ok(isProduction);
                var results = _dashboardProvider.Value.GetDashboardKpis();
                return Ok(results);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
