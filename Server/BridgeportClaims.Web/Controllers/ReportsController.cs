using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.Reports;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/reports")]
    public class ReportsController : BaseApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IReportsDataProvider _reportsDataProvider;

        public ReportsController(IReportsDataProvider reportsDataProvider)
        {
            _reportsDataProvider = reportsDataProvider;
        }

        [HttpPost]
        [Route("accounts-receivable")]
        public IHttpActionResult GetAccountsReceivableReport()
        {
            try
            {
                return Ok(_reportsDataProvider.GetAccountsReceivableReport());
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
