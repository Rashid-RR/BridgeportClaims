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
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Entities.DomainModels;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/users")]
    public class UsersController : BaseApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IAspNetUsersProvider _aspNetUsersProvider;
        private readonly IAssignUsersToRolesProvider _assignUsersToRolesProvider;
        private readonly IRepository<AspNetUsers> _usersRepository;
        private readonly IUsersProvider _usersProvider;

        public UsersController(IAspNetUsersProvider aspNetUsersProvider, 
            IAssignUsersToRolesProvider assignUsersToRolesProvider, 
            IRepository<AspNetUsers> usersRepository, 
            IUsersProvider usersProvider)
        {
            _aspNetUsersProvider = aspNetUsersProvider;
            _assignUsersToRolesProvider = assignUsersToRolesProvider;
            _usersRepository = usersRepository;
            _usersProvider = usersProvider;
        }

        [HttpPost]
        [Route("get-users-to-assign")]
        public async Task<IHttpActionResult> GetUsersToAssign()
        {
            try
            {
                return await Task.Run(() => Ok(_usersProvider.GetUsers()));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("get-users")]
        public async Task<IHttpActionResult> GetAllUsers()
        {
            try
            {
                return await Task.Run(() =>
                {
                    var users = _usersRepository?.GetAll()?.OrderBy(x => x.LastName)
                        .ThenBy(y => y.FirstName).Select(s => new {OwnerId = s.Id, Owner = s.LastName + ", " + s.FirstName}).ToList();
                    return Ok(users);
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
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
        public async Task<IHttpActionResult> UpdateName(string id, string firstName = null, 
            string lastName = null, string extension = null)
        {
            try
            {
                if (firstName.IsNullOrWhiteSpace() && lastName.IsNullOrWhiteSpace() && extension.IsNullOrWhiteSpace())
                    throw new Exception($"Error, the {nameof(firstName)} parameter and the {nameof(lastName)}" +
                                        " parameter cannot both be null or empty.");
                return await Task.Run(() =>
                {
                    _aspNetUsersProvider.UpdatePersonalData(id, firstName, lastName, extension);
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
