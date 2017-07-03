using NLog;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Web.Infrastructure;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using c = BridgeportClaims.Common.StringConstants.Constants;
using System.Net.Http;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/account")]
    public class AccountsController : BaseApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private ApplicationUserManager _userManager;
        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        public AccountsController() { }

        public AccountsController(ApplicationUserManager userManager, ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            try
            {
                var externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);
                var user = UserManager.FindByName(User.Identity.Name);
                return new UserInfoViewModel
                {
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    HasRegistered = externalLogin == null,
                    LoginProvider = externalLogin?.LoginProvider,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    RegisteredDate = user.RegisteredDate
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("create")]
        public async Task<IHttpActionResult> CreateUser(CreateUserBindingModel createUserModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var user = new ApplicationUser
                {
                    UserName = createUserModel.Username,
                    Email = createUserModel.Email,
                    FirstName = createUserModel.FirstName,
                    LastName = createUserModel.LastName,
                    RegisteredDate = DateTime.Now.Date
                };
                // Register to the Database
                var addUserResult = await AppUserManager.CreateAsync(user, createUserModel.Password);
                if (!addUserResult.Succeeded)
                    return GetErrorResult(addUserResult);
                var locationHeader = new Uri(Url.Link(c.GetUserByIdAction, new {id = user.Id}));
                // Email
                var code = await AppUserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var callbackUrl = new Uri(Url.Link(c.ConfirmEmailRouteAction, new {userId = user.Id, code}));
                // This is so wrong. We're using the Full Name for the Email Subject, and the Absolute Activation Uri for the Email body.
                await AppUserManager.SendEmailAsync(user.Id, $"{user.FirstName} {user.LastName}",
                    callbackUrl.AbsoluteUri);
                return Created(locationHeader, TheModelFactory.Create(user));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId = "", string code = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
                {
                    ModelState.AddModelError("", "User Id and Code are required");
                    return BadRequest(ModelState);
                }
                var result = await AppUserManager.ConfirmEmailAsync(userId, code);
                return result.Succeeded ? Ok() : GetErrorResult(result);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            try
            {
                return Ok(AppUserManager.Users.ToList().Select(u => TheModelFactory.Create(u)));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpPost]
        [Route("user/{id:guid}", Name = c.GetUserByIdAction)]
        public async Task<IHttpActionResult> GetUser(string id)
        {
            try
            {
                var user = await AppUserManager.FindByIdAsync(id);
                if (user != null)
                    return Ok(TheModelFactory.Create(user));
                return NotFound();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpPost]
        [Route("user/{username}")]
        public async Task<IHttpActionResult> GetUserByName(string username)
        {
            try
            {
                var user = await AppUserManager.FindByNameAsync(username);
                if (null != user)
                    return Ok(TheModelFactory.Create(user));
                return NotFound();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpPut]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var result = await AppUserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                    model.NewPassword);
                return !result.Succeeded ? GetErrorResult(result) : Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [Route("user/{id:guid}")]
        // [DenyAction]// Jut kiggin

        public async Task<IHttpActionResult> DeleteUser(string id)
        {
            try
            {
                var appUser = await AppUserManager.FindByIdAsync(id);
                if (appUser != null)
                {
                    var result = await AppUserManager.DeleteAsync(appUser);
                    return !result.Succeeded ? GetErrorResult(result) : Ok();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; private set; }
            public string ProviderKey { get; private set; }
            private string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                try
                {
                    IList<Claim> claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider)
                    };
                    if (UserName != null)
                    {
                        claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                    }

                    return claims;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    throw;
                }
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                try
                {
                    var providerKeyClaim = identity?.FindFirst(ClaimTypes.NameIdentifier);

                    if (string.IsNullOrEmpty(providerKeyClaim?.Issuer) ||
                        string.IsNullOrEmpty(providerKeyClaim.Value))
                        return null;


                    if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                        return null;

                    return new ExternalLoginData
                    {
                        LoginProvider = providerKeyClaim.Issuer,
                        ProviderKey = providerKeyClaim.Value,
                        UserName = identity.FindFirstValue(ClaimTypes.Name)
                    };
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    throw;
                }
            }
        }
    }
}