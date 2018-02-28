using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.AdminFunctions;
using BridgeportClaims.Web.Models;
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
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("delete-firewall-setting")]
        public async Task<IHttpActionResult> DeleteFirewallSetting(string ruleName)
        {
            try
            {
                return await Task.Run(() =>
                {
                    _adminFunctionsProvider.DeleteFirewallSetting(ruleName);
                    return Ok(new {message = $"The firewall rule '{ruleName}' was removed successfully."});
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("create-firewall-setting")]
        public async Task<IHttpActionResult> CreateFirewallSetting(CreateFirewallSettingModel model)
        {
            try
            {
                return await Task.Run(() =>
                {
                    if (model.RuleName.IsNullOrWhiteSpace())
                        throw new ArgumentNullException(nameof(model.RuleName));
                    if (model.StartIpAddress.IsNullOrWhiteSpace())
                        throw new ArgumentNullException(nameof(model.StartIpAddress));
                    if (model.EndIpAddress.IsNullOrWhiteSpace())
                        throw new ArgumentNullException(nameof(model.EndIpAddress));
                    _adminFunctionsProvider.AddFirewallSetting(model.RuleName, model.StartIpAddress,
                        model.EndIpAddress);
                    return Ok(new {message = $"The firewall rule '{model.RuleName}' was created successfully."});
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
