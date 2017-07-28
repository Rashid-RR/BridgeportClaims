﻿using NLog;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Web.Infrastructure;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using BridgeportClaims.Web.Attributes;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using c = BridgeportClaims.Common.StringConstants.Constants;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/account")]
    [SuppressMessage("ReSharper", "ArrangeAccessorOwnerBody")]
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

        private string BaseUri => Request.RequestUri.GetLeftPart(UriPartial.Authority);
        private bool IsSecure => Request.RequestUri.Scheme.ToLower() == "https";

        public AccountsController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("forgotpassword")]
        public async Task<IHttpActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var user = await UserManager.FindByNameAsync(model.Email);
                if (null == user)
                    return BadRequest("The email that you have entered does not exist within the system.");
                // If user has to activate his email to confirm his account, the use code listing below
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                    return BadRequest(
                        "You must confirm your email address from your registration before confirming your password");

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = new Uri(Url.Link(c.ResetPasswordRouteAction, new {userId = user.Id, code}));
                var callbackUrl = GetCallbackUrlForEmail(EmailType.ResetPassword, user.Id, code);
                await UserManager.SendEmailAsync(user.Id, $"{user.FirstName} {user.LastName}",
                    callbackUrl.AbsoluteUri);
                return Ok(new {message = "Please check your Email. An Email has been sent to Reset your Password"});

                // If we got this far, something failed, redisplay form
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        private Uri GetCallbackUrlForEmail(EmailType type, string userId, string code)
        {
            var route = string.Empty;
            switch (type)
            {
                case EmailType.ResetPassword:
                    route = c.ResetPasswordClientRoute;
                    break;
                case EmailType.Registration:
                    route = c.ConfirmEmailClientRoute;
                    break;
            }
            var baseUrl = BaseUri;
            var serverLocalHostName = cs.GetAppSetting(c.ServerLocalHostNameKey);
            var secureServerLocalHostName = cs.GetAppSetting(c.SecureServerLocalHostNameKey);
            var clientLocalHostName = cs.GetAppSetting(c.ClientLocalHostNameKey);
            if (!BaseUri.Contains(serverLocalHostName) && !BaseUri.Contains(secureServerLocalHostName))
                return ConstructUri(baseUrl, route, userId, code);
            if (IsSecure)
            {
                var containsHttps = baseUrl?.ToLower().Contains("https");
                if (containsHttps.HasValue && containsHttps.Value)
                {
                    baseUrl = BaseUri.Replace("https", "http").Replace(secureServerLocalHostName, clientLocalHostName);
                }
            }
            else
            {
                baseUrl = BaseUri.Replace(serverLocalHostName, clientLocalHostName);
            }
            return ConstructUri(baseUrl, route, userId, code);
        }

        private static Uri ConstructUri(string baseUrl, string route, string userId, string code)
            => new Uri($"{baseUrl}/#/{route}/?userId={userId}&code={code}");

        [HttpPost]
        [AllowAnonymous]
        [Route("resetpassword", Name = c.ResetPasswordRouteAction)]
        public async Task<IHttpActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var user = await UserManager.FindByIdAsync(model.UserId);
                if (null == user)
                {
                    // Don't reveal that the user does not exist (security hole)
                    return Ok();
                }
                var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                if (result.Succeeded)
                    return Ok(new {message = "The Password was Reset Successfully"});
                string error = null;
                if (!result.Errors.Any())
                {
                    error = result.Errors.Count() > 1
                        ? string.Join(", ", result.Errors.SelectMany(sm => sm))
                        : result.Errors.Select(x => x).FirstOrDefault();
                }
                Logger.Error(error);
                return GetBadRequestFormattedErrorMessages(result);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpGet]
        [Route("UserInfo")]
        public async Task<UserInfoViewModel> GetUserInfo()
        {
            try
            {
                return await Task.Run(() =>
                {
                    var externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);
                    var user = UserManager.FindByName(User.Identity.Name);
                    return new UserInfoViewModel
                    {
                        Id = User.Identity.GetUserId(),
                        Email = user.Email,
                        EmailConfirmed = user.EmailConfirmed,
                        HasRegistered = externalLogin == null,
                        LoginProvider = externalLogin?.LoginProvider,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        RegisteredDate = user.RegisteredDate,
                        Roles = user.Roles.Join(AppRoleManager.Roles, ur => ur.RoleId,
                            r => r.Id, (ur, r) => r.Name).ToList()
                    };
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("create")]
        public async Task<IHttpActionResult> CreateUser(CreateUserBindingModel createUserModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return GetBadRequestFormattedErrorMessages();
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
                // Generate link for the email.
                var callbackUrl = GetCallbackUrlForEmail(EmailType.Registration, user.Id, code);
                // the line below can be uncommented, in place of the two lines above, to generate a link directly to the API.
                // var callbackUrl = new Uri(Url.Link(c.ConfirmEmailRouteAction, new { userId = user.Id, code }));
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
        [Route("ConfirmEmail", Name = c.ConfirmEmailRouteAction)]
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

        [HttpGet]
        [Route("users")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> GetUsers()
        {
            try
            {
                return await Task.Run(() =>
                {
                    return Ok(AppUserManager.Users.ToList().Select(u => TheModelFactory.Create(u)));
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpPost]
        [Route("user/{id:guid}", Name = c.GetUserByIdAction)]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}")]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {
            try
            {
                var appUser = await AppUserManager.FindByIdAsync(id);
                if (null == appUser)
                    return Content(HttpStatusCode.InternalServerError,
                        new {message = "Error. The user was not found."});
                var result = await AppUserManager.DeleteAsync(appUser);
                return !result.Succeeded ? GetErrorResult(result) : Ok(new {message="The User was Deleted Successfully."});
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.InternalServerError, new {message = ex.Message});
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