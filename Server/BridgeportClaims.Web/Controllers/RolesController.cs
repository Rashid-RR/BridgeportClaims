using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using NLog;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using c = BridgeportClaims.Common.StringConstants.Constants;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/roles")]
    public class RolesController : BaseApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [Route("{id:guid}", Name = c.GetRoleByIdAction)]
        public async Task<IHttpActionResult> GetRole(string id)
        {
            var role = await AppRoleManager.FindByIdAsync(id);
            if (role != null)
                return Ok(TheModelFactory.Create(role));
            return NotFound();

        }

        [Route("", Name = c.GetAllRolesAction)]
        public IHttpActionResult GetAllRoles()
        {
            var roles = AppRoleManager.Roles;

            return Ok(roles);
        }

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

                var locationHeader = new Uri(Url.Link(c.GetRoleByIdAction, new {id = role.Id}));
                return Created(locationHeader, TheModelFactory.Create(role));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

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
                Logger.Error(ex);
                throw;
            }
        }

        [Route(c.ManageUsersInRoleAction)]
        public async Task<IHttpActionResult> ManageUsersInRole(UsersInRoleModel model)
        {
            try
            {
                var role = await AppRoleManager.FindByIdAsync(model.Id);
                if (role == null)
                {
                    ModelState.AddModelError("", "Role does not exist");
                    return BadRequest(ModelState);
                }

                foreach (var user in model.EnrolledUsers)
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

                foreach (var user in model.RemovedUsers)
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

                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
