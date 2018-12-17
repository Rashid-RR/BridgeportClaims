using NLog;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Common.Constants;
using BridgeportClaims.Web.Framework.Attributes;
using BridgeportClaims.Web.Framework.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/roles")]
    public class RolesController : BaseApiController
    {
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);

        [HttpGet]
        [Route("{id:guid}", Name = StringConstants.GetRoleByIdAction)]
        public async Task<IHttpActionResult> GetRole(string id)
        {
            try
            {
                var role = await AppRoleManager.FindByIdAsync(id);
                if (role != null)
                    return Ok(TheModelFactory.Create(role));
                return NotFound();
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("", Name = StringConstants.GetAllRolesAction)]
        public IHttpActionResult GetAllRoles()
        {
            var roles = AppRoleManager.Roles;

            return Ok(roles);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IHttpActionResult> Create(CreateRoleBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var role = new IdentityRole { Name = model.Name};
                var result = await AppRoleManager.CreateAsync(role);
                if (!result.Succeeded)
                    return GetErrorResult(result);

                var locationHeader = new Uri(Url.Link(StringConstants.GetRoleByIdAction, new {id = role.Id}));
                return Created(locationHeader, TheModelFactory.Create(role));
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [DenyAction]
        [Route("{id:guid}")]
        public async Task<IHttpActionResult> DeleteRole(string id)
        {
            try
            {
                var role = await AppRoleManager.FindByIdAsync(id);
                if (role == null) return NotFound();
                var result = await AppRoleManager.DeleteAsync(role);
                return !result.Succeeded ? GetErrorResult(result) : Ok();
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}/roles")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignRolesToUser([FromUri] string id, [FromBody] string[] rolesToAssign)
        {
            try
            {
                var appUser = await AppUserManager.FindByIdAsync(id);
                if (appUser == null)
                    return NotFound();
                var currentRoles = await AppUserManager.GetRolesAsync(appUser.Id);
                var rolesNotExists = rolesToAssign.Except(AppRoleManager.Roles.Select(x => x.Name)).ToArray();
                if (rolesNotExists.Any())
                {
                    ModelState.AddModelError("",
                        $"Roles '{string.Join(",", rolesNotExists)}' does not exixts in the system");
                    return BadRequest(ModelState);
                }
                var removeResult = await AppUserManager.RemoveFromRolesAsync(appUser.Id, currentRoles.ToArray());
                if (!removeResult.Succeeded)
                {
                    ModelState.AddModelError("", "Failed to remove user roles");
                    return BadRequest(ModelState);
                }
                var addResult = await AppUserManager.AddToRolesAsync(appUser.Id, rolesToAssign);
                if (addResult.Succeeded) return Ok();
                ModelState.AddModelError("", "Failed to add user roles");
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route(StringConstants.ManageUsersInRoleAction)]
        public async Task<IHttpActionResult> ManageUsersInRole(UsersInRoleModel model)
        {
            try
            {
                var role = await AppRoleManager.FindByIdAsync(model.Id);
                if (null == role)
                {
                    ModelState.AddModelError("", "Role does not exist");
                    return BadRequest(ModelState);
                }

                foreach (var user in model.EnrolledUsers ?? Enumerable.Empty<string>())
                {
                    var appUser = await AppUserManager.FindByIdAsync(user);
                    if (null == appUser)
                    {
                        ModelState.AddModelError("", $"User: {user} does not exists");
                        continue;
                    }
                    if (AppUserManager.IsInRole(user, role.Name)) continue;
                    var result = await AppUserManager.AddToRoleAsync(user, role.Name);

                    if (!result.Succeeded)
                        ModelState.AddModelError("", $"User: {user} could not be added to role");
                }

                foreach (var user in model.RemovedUsers ?? Enumerable.Empty<string>())
                {
                    var appUser = await AppUserManager.FindByIdAsync(user);
                    if (appUser == null)
                    {
                        ModelState.AddModelError("", $"User: {user} does not exists");
                        continue;
                    }
                    var result = await AppUserManager.RemoveFromRoleAsync(user, role.Name);
                    if (!result.Succeeded)
                        ModelState.AddModelError("", $"User: {user} could not be removed from role");
                }
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok( new {message ="User Role(s) saved Successfully"});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
