using NLog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Common.Caching;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.Accounts;
using BridgeportClaims.Data.DataProviders.UserRoles;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/users")]
    public class UsersController : BaseApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IAspNetUsersProvider _aspNetUsersProvider;
        private readonly IAssignUsersToRolesProvider _assignUsersToRolesProvider;
        private readonly IMemoryCacher _memoryCacher;

        public UsersController(IAspNetUsersProvider aspNetUsersProvider, IAssignUsersToRolesProvider assignUsersToRolesProvider)
        {
            _aspNetUsersProvider = aspNetUsersProvider;
            _assignUsersToRolesProvider = assignUsersToRolesProvider;
            _memoryCacher = MemoryCacher.Instance;
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
                _memoryCacher.Delete(id);
                await AppUserManager.RemoveFromRolesAsync(id, roles.ToArray());
                await AppUserManager.SetLockoutEnabledAsync(id, true);
                await AppUserManager.SetLockoutEndDateAsync(id, DateTimeOffset.UtcNow.AddYears(200));
                await AppUserManager.AccessFailedAsync(id);
                return Ok(new {message = "User Deactivated Successfully"});
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
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
                _memoryCacher.Delete(id);
                if (!isInUserRole)
                    await AppUserManager.AddToRoleAsync(id, "User");
                await AppUserManager.SetLockoutEndDateAsync(id, DateTimeOffset.UtcNow.AddYears(-1));
                return Ok(new {message = "User Activated Successfully"});
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("unassign/{id:guid}")]
        public async Task<IHttpActionResult> UnassignRole(string id, string roleName)
        {
            try
            {
                if (id.IsNullOrWhiteSpace())
                    throw new ArgumentNullException(nameof(id));
                if (roleName.IsNullOrWhiteSpace())
                    throw new ArgumentNullException(nameof(roleName));
                var role = await AppRoleManager.FindByNameAsync(roleName);
                await AppRoleManager.DeleteAsync(role);
                var userName = User.Identity.Name;
                return Ok(new {message = $"The {role.Name} role was removed from {userName} successfully." });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        [HttpPost]
        [Route("assign/{id:guid}")]
        public async Task<IHttpActionResult> AssignRole(string id, string roleName)
        {
            try
            {
                if (id.IsNullOrWhiteSpace())
                    throw new ArgumentNullException(nameof(id));
                if (roleName.IsNullOrWhiteSpace())
                    throw new ArgumentNullException(nameof(roleName));
                var role = await AppRoleManager.FindByNameAsync(roleName);
                _assignUsersToRolesProvider.AssignUserToRole(id, role.Id);
                var userName = User.Identity.Name;
                return Ok(new{message=$"The {role.Name} role was assigned to {userName} successfully."});
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
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
                    _memoryCacher.Delete(id);
                    _aspNetUsersProvider.UpdateFirstOrLastName(id, firstName, lastName);
                    return Ok(new {message = "Name has been Updated Successfully"});
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
