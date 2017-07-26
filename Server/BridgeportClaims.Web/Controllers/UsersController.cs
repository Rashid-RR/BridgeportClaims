using NLog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.Accounts;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/users")]
    public class UsersController : BaseApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IAspNetUsersProvider _aspNetUsersProvider;

        public UsersController(IAspNetUsersProvider aspNetUsersProvider)
        {
            _aspNetUsersProvider = aspNetUsersProvider;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("deactivate/{id:guid}")]
        public async Task<IHttpActionResult> Deactivate(string id)
        {
            try
            {
                // Remove from all roles.
                var isAdmin = await AppUserManager.IsInRoleAsync(id, "Admin");
                var isUser = await AppUserManager.IsInRoleAsync(id, "User");
                var roles = new List<string>();
                if (isAdmin)
                    roles.Add("Admin");
                if (isUser)
                    roles.Add("User");
                await AppUserManager.RemoveFromRolesAsync(id, roles.ToArray());
                await AppUserManager.SetLockoutEnabledAsync(id, true);
                await AppUserManager.SetLockoutEndDateAsync(id, DateTimeOffset.UtcNow.AddYears(200));
                await AppUserManager.AccessFailedAsync(id);
                return Ok(new {message = "User Deactivated Successfully"});
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("activate/{id:guid}")]
        public async Task<IHttpActionResult> Activate(string id)
        {
            try
            {
                var isInUserRole = await AppUserManager.IsInRoleAsync(id, "User");
                if (!isInUserRole)
                    await AppUserManager.AddToRoleAsync(id, "User");
                await AppUserManager.SetLockoutEndDateAsync(id, DateTimeOffset.UtcNow.AddYears(-1));
                return Ok(new {message = "User Activated Successfully"});
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("updatename/{id:guid}")]
        public async Task<IHttpActionResult> UpdateName(string id, string firstName = null, string lastName = null)
        {
            try
            {
                if (firstName.IsNullOrWhiteSpace() && lastName.IsNullOrWhiteSpace())
                    throw new Exception($"Error, the {nameof(firstName)} parameter and the {nameof(lastName)}" +
                                        " parameter cannot both be null or empty.");
                return await Task.Run(() =>
                {
                    _aspNetUsersProvider.UpdateFirstOrLastName(id, firstName, lastName);
                    return Ok(new {message = "Name has been Updated Successfully"});
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }
    }
}
