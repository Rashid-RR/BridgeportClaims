using System;
using System.Net.Http;
using System.Web.Http;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    public class BaseApiController : ApiController
    {
        private ModelFactory _modelFactory;
        private readonly ApplicationUserManager _appUserManager = null;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected ApplicationUserManager AppUserManager
        {
            get
            {
                try
                {
                    return _appUserManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    throw;
                }
            }
        }

    protected ModelFactory TheModelFactory => _modelFactory ??
                                                  (_modelFactory = new ModelFactory(Request, AppUserManager));

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            try
            {
                if (result == null)
                    return InternalServerError();

                if (result.Succeeded)
                    return null;

                if (null != result.Errors)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                        Logger.Error(error);
                    }
                }

                if (ModelState.IsValid)
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
