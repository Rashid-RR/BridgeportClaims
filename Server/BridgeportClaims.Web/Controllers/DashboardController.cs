using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.Dashboards;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/dashboard")]
    public class DashboardController : BaseApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IDashboardProvider _dashboardProvider;

        public DashboardController(IDashboardProvider dashboardProvider)
        {
            _dashboardProvider = dashboardProvider;
        }

        [HttpPost]
        [Route("kpi")]
        public IHttpActionResult GetDashboardKpis()
        {
            try
            {
                var results = _dashboardProvider.GetDashboardKpis();
                return Ok(results);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
