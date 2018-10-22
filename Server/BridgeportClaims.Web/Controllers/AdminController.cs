using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.AdminFunctions;
using BridgeportClaims.Data.DataProviders.Clients;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/admin")]
    public class AdminController : BaseApiController
    {
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);
        private readonly Lazy<IAdminFunctionsProvider> _adminFunctionsProvider;
        private readonly Lazy<IClientDataProvider> _clientDataProvider;

        public AdminController(Lazy<IAdminFunctionsProvider> adminFunctionsProvider,
            Lazy<IClientDataProvider> clientDataProvider)
        {
            _adminFunctionsProvider = adminFunctionsProvider;
            _clientDataProvider = clientDataProvider;
        }

        [HttpPost]
        [Route("get-referral-types")]
        public IHttpActionResult GetReferralTypes()
        {
            try
            {
                var types = _clientDataProvider.Value.GetReferralTypes();
                return Ok(types);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("get-invoice-amounts")]
        public IHttpActionResult GetInvoiceAmounts(InvoiceAmountsModel model)
        {
            try
            {
                return Ok(_adminFunctionsProvider.Value.GetInvoiceAmounts(model.ClaimId, model.RxNumber,
                              model.RxDate.ToNullableFormattedDateTime(), model.InvoiceNumber) ??
                          new List<InvoiceAmountDto>());
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("update-billed-amount")]
        public IHttpActionResult UpdateBilledAmount(int prescriptionId, decimal billedAmount)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                _adminFunctionsProvider.Value.UpdateBilledAmount(prescriptionId, billedAmount, userId);
                return Ok(new {message = $"The billed amount was updated to {billedAmount:C} successfully."});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("firewall")]
        public IHttpActionResult GetFirewallSettings()
        {
            try
            {
                var results = _adminFunctionsProvider.Value.GetFirewallSettings();
                return Ok(results);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        [HttpPost]
        [Route("delete-firewall-setting")]
        public IHttpActionResult DeleteFirewallSetting(string ruleName)
        {
            try
            {
                _adminFunctionsProvider.Value.DeleteFirewallSetting(ruleName);
                return Ok(new {message = $"The firewall rule '{ruleName}' was removed successfully."});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        [HttpPost]
        [Route("create-firewall-setting")]
        public IHttpActionResult CreateFirewallSetting(CreateFirewallSettingModel model)
        {
            try
            {
                if (model.RuleName.IsNullOrWhiteSpace())
                    throw new ArgumentNullException(nameof(model.RuleName));
                if (model.StartIpAddress.IsNullOrWhiteSpace())
                    throw new ArgumentNullException(nameof(model.StartIpAddress));
                if (model.EndIpAddress.IsNullOrWhiteSpace())
                    throw new ArgumentNullException(nameof(model.EndIpAddress));
                _adminFunctionsProvider.Value.AddFirewallSetting(model.RuleName, model.StartIpAddress,
                    model.EndIpAddress);
                return Ok(new {message = $"The firewall rule '{model.RuleName}' was created successfully."});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }
    }
}
