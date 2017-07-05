using NLog;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.Accounts;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/users")]
    public class UsersController : BaseApiController
    {
        private readonly IAspNetUsersProvider _aspNetUsersProvider;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public UsersController(IAspNetUsersProvider aspNetUsersProvider)
        {
            _aspNetUsersProvider = aspNetUsersProvider;
        }

        [HttpPost]
        [Route("deactivate/{id:guid}")]
        public async Task<IHttpActionResult> Deactivate(string id)
        {
            try
            {
                return await Task.Run(() =>
                {
                    _aspNetUsersProvider.DeactivateUser(id);
                    return Ok(new {message = "User Deactivated Successfully"});
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpPost]
        [Route("activate/{id:guid}")]
        public async Task<IHttpActionResult> Activate(string id)
        {
            try
            {
                return await Task.Run(() =>
                {
                    _aspNetUsersProvider.ActivateUser(id);
                    return Ok(new { message = "User Activated Successfully" });
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpPost]
        [Route("updatename/{id:guid}")]
        public async Task<IHttpActionResult> UpdateName(string id, string firstName, string lastName)
        {
            try
            {
                return await Task.Run(() =>
                {
                    _aspNetUsersProvider.UpdateFirstOrLastName(id, firstName, lastName);
                    return Ok(new {message = "Name has been Updated Successfully"});
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
