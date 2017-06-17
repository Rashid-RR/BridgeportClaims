using System.Net.Http;
using System.Web.Http;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace BridgeportClaims.Web.Controllers
{
    public class BaseApiController : ApiController
    {

        private ModelFactory _modelFactory;
        private readonly ApplicationUserManager _appUserManager = null;

        protected ApplicationUserManager AppUserManager => 
            _appUserManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();

        protected ModelFactory TheModelFactory => _modelFactory ??
                                                  (_modelFactory = new ModelFactory(Request, AppUserManager));

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
                return InternalServerError();

            if (result.Succeeded)
                return null;

            if (result.Errors != null)
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error);

            if (ModelState.IsValid)
                // No ModelState errors are available to send, so just return an empty BadRequest.
                return BadRequest();

            return BadRequest(ModelState);
        }
    }
}
