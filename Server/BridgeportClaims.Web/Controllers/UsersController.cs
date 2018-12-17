using NLog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.Accounts;
using BridgeportClaims.Data.DataProviders.UserRoles;
using BridgeportClaims.Data.DataProviders.Users;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User,Client")]
    [RoutePrefix("api/users")]
    public class UsersController : BaseApiController
    {
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);
        private readonly Lazy<IAspNetUsersProvider> _aspNetUsersProvider;
        private readonly Lazy<IAssignUsersToRolesProvider> _assignUsersToRolesProvider;
        private readonly Lazy<IUsersProvider> _usersProvider;

        public UsersController(Lazy<IAspNetUsersProvider> aspNetUsersProvider, 
            Lazy<IAssignUsersToRolesProvider> assignUsersToRolesProvider,
            Lazy<IUsersProvider> usersProvider)
        {
            _aspNetUsersProvider = aspNetUsersProvider;
            _assignUsersToRolesProvider = assignUsersToRolesProvider;
            _usersProvider = usersProvider;
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        [Route("get-users-to-assign")]
        public async Task<IHttpActionResult> GetUsersToAssign()
        {
            try
            {
                return await Task.Run(() => Ok(_usersProvider.Value.GetUsers()));
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        [Route("get-users")]
        public IHttpActionResult GetAllUsers()
        {
            try
            {
                var users = _aspNetUsersProvider.Value.GetAllUsers()
                    ?.OrderBy(o => o.LastName)
                    .ThenBy(t => t.FirstName)
                    .Select(x => new {OwnerId = x.Id, Owner = x.LastName + ", " + x.FirstName});
                if (null == users)
                {
                    return Content(HttpStatusCode.InternalServerError, new {message = "No users were located."});
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("deactivate/{id:guid}")]
        public async Task<IHttpActionResult> Deactivate(string id)
        {
            try
            {
                var rolesToRemove = new List<string>();
                var roles = AppRoleManager?.Roles?.Select(x => x.Name).ToArray();
                if (null == roles)
                    throw new Exception("Error, no roles were found in the system.");
                foreach (var role in roles)
                {
                    var isInRole = await AppUserManager.IsInRoleAsync(id, role);
                    if (isInRole)
                        rolesToRemove.Add(role);
                }
                await AppUserManager.RemoveFromRolesAsync(id, rolesToRemove.ToArray());
                await AppUserManager.SetLockoutEnabledAsync(id, true);
                await AppUserManager.SetLockoutEndDateAsync(id, DateTimeOffset.UtcNow.AddYears(200));
                await AppUserManager.AccessFailedAsync(id);
                return Ok(new {message = "User Deactivated Successfully"});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
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
                if (!isInUserRole)
                    await AppUserManager.AddToRoleAsync(id, "User");
                await AppUserManager.SetLockoutEndDateAsync(id, DateTimeOffset.UtcNow.AddYears(-1));
                return Ok(new {message = "User Activated Successfully"});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
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
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("assign/{id:guid}")]
        public async Task<IHttpActionResult> AssignRole(string id, string roleName)
        {
            try
            {
                if (id.IsNullOrWhiteSpace())
                    throw new ArgumentNullException(nameof(id));
                if (roleName.IsNullOrWhiteSpace())
                    throw new ArgumentNullException(nameof(roleName));
                var role = await AppRoleManager.FindByNameAsync(roleName).ConfigureAwait(false);
                _assignUsersToRolesProvider.Value.AssignUserToRole(id, role.Id);
                var user = await AppUserManager.FindByIdAsync(id).ConfigureAwait(false);
                var userName = user.UserName;
                return Ok(new {message = $"The {role.Name} role was assigned to {userName} successfully."});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }


        [HttpPost]
        [Route("updatename/{id:guid}")]
        public async Task<IHttpActionResult> UpdateName(string id, string firstName, 
            string lastName, string extension = null)
        {
            try
            {
                if (firstName.IsNullOrWhiteSpace() || lastName.IsNullOrWhiteSpace())
                    throw new Exception($"Error, neither the {nameof(firstName)} parameter, nor the {nameof(lastName)}" +
                                        " parameter can be null or empty.");
                if (extension.IsNotNullOrWhiteSpace() && extension?.ToLower() == "null")
                {
                    extension = null;
                }
                var appUser = await AppUserManager.FindByIdAsync(id);
                appUser.FirstName = firstName;
                appUser.LastName = lastName;
                appUser.Extension = extension;
                await AppUserManager.UpdateAsync(appUser);
                return Ok(new {message = "Name has been Updated Successfully"});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }
    }
}
