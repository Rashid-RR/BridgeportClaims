using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.Clients;
using BridgeportClaims.Data.Dtos;
using Microsoft.AspNet.Identity;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "Client")]
    [RoutePrefix("api/client")]
    public class ClientController : BaseApiController
    {
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);
        private readonly Lazy<IClientDataProvider> _clientDataProvider;

        public ClientController(Lazy<IClientDataProvider> clientDataProvider)
        {
            _clientDataProvider = clientDataProvider;
        }

        [HttpPost]
        [Route("get-states")]
        public IHttpActionResult GetStates()
        {
            try
            {
                var states = _clientDataProvider.Value.GetUsStates();
                return Ok(states);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("get-user-data")]
        public IHttpActionResult GetClientData()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var user = AppUserManager.FindById(userId);
                return Ok(new { user.FirstName, user.LastName, user.FullName, user.RegisteredDate, user.Email});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("insert-referral")]
        public IHttpActionResult InsertReferral(ReferralDto model)
        {
            try
            {
                var referrerId = User.Identity.GetUserId();
                model.ReferredBy = referrerId;
                _clientDataProvider.Value.InsertReferral(model);
                return Ok(new {message = "Referral added successfully."});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
