using System;
using System.Net;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "Client")]
    [RoutePrefix("api/client")]
    public class ClientController : BaseApiController
    {
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);

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
    }
}
