using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.AdminFunctions;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/admin")]
    public class AdminController : BaseApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IAdminFunctionsProvider _adminFunctionsProvider;

        public AdminController(IAdminFunctionsProvider adminFunctionsProvider)
        {
            _adminFunctionsProvider = adminFunctionsProvider;
        }

        [HttpPost]
        [Route("firewall")]
        public async Task<IHttpActionResult> GetFirewallSettings()
        {
            try
            {
                return await Task.Run(() =>
                {
                    var results = _adminFunctionsProvider.GetFirewallSettings();
                    return Ok(results);
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
