using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using BridgeportClaims.Web.Infrastructure;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class BaseApiController : ApiController
    {
        private ModelFactory _modelFactory;
        private readonly ApplicationUserManager _appUserManager = null;
        private readonly ApplicationRoleManager _appRoleManager = null;
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);

        protected ApplicationRoleManager AppRoleManager
        {
            get
            {
                try
                {
                    return _appRoleManager ?? Request?.GetOwinContext()?.GetUserManager<ApplicationRoleManager>();
                }
                catch (Exception ex)
                {
                    Logger.Value.Error(ex);
                    throw;
                }
            }
        }

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
                    Logger.Value.Error(ex);
                    throw;
                }
            }
        }

        protected ModelFactory TheModelFactory
        {
            get
            {
                try
                {
                    return _modelFactory ??
                           (_modelFactory = new ModelFactory(Request, AppUserManager));
                }
                catch (Exception ex)
                {
                    Logger.Value.Error(ex);
                    throw;
                }
            }
        }

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (null == result)
                throw new ArgumentNullException(nameof(result));
            if (result.Succeeded)
            {
                return null;
            }
            if (result.Errors == null)
            {
                return ModelState.IsValid ? BadRequest() : GetBadRequestFormattedErrorMessages();
            }
            foreach (var error in result.Errors)
                if (error.StartsWith("Name ") && error.EndsWith(" is already taken.")) { } // Do nothing
                else // Else add messages to the ModelState
                    ModelState.AddModelError("", error);
            return ModelState.IsValid ? BadRequest() : GetBadRequestFormattedErrorMessages();
        }

        protected IHttpActionResult GetBadRequestFormattedErrorMessages(IdentityResult result)
        {
            string error_description = null;
            if (null != result?.Errors)
            {
                error_description = result.Errors.Count() > 1
                    ? string.Join(", ", result.Errors.SelectMany(sm => sm))
                    : result.Errors.Select(x => x).FirstOrDefault();
            }
            return BadRequest(error_description ?? "An error occurred.");
        }

        protected IHttpActionResult GetBadRequestFormattedErrorMessages()
        {
            var error_description = string.Join(", ",
                ModelState.Values.SelectMany(sm => sm.Errors).Select(err => err.ErrorMessage));
            return BadRequest(error_description);
        }
    }
}
