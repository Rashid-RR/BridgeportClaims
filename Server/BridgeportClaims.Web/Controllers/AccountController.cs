using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using BridgeportClaims.Web.Models;
using BridgeportClaims.Web.Providers;
using BridgeportClaims.Web.Results;
using FluentNHibernate.Utils;
using Microsoft.Owin.Security.DataProtection;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/Account")]
    public class AccountController : BaseApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public AccountController() { }

        public AccountController(ApplicationUserManager userManager, ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; }

        [Authorize(Roles = "Admin")]
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
                    JoinDate = DateTime.Now.Date,
                };
                var addUserResult = await AppUserManager.CreateAsync(user, createUserModel.Password);
                if (!addUserResult.Succeeded)
                    return GetErrorResult(addUserResult);

                // Email Confirmation code.
                //var magicCode = await AppUserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var provider = new DpapiDataProtectionProvider();
                AppUserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser, string>(provider.Create("EmailConfirmation"));
                var magicCode = await AppUserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { userId = user.Id, code = magicCode }));

                // This is so wrong. We're using the Full Name for the Email Subject, and the Absolute Activation Uri for the Email body.
                await AppUserManager.SendEmailAsync(user.Id, $"{user.FirstName} {user.LastName}", callbackUrl.AbsoluteUri);
                var locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));
                return Created(locationHeader, TheModelFactory.Create(user));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId = "", string code = "")
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }
            var result = await AppUserManager.ConfirmEmailAsync(userId, code);
            return result.Succeeded ? Ok() : GetErrorResult(result);
        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}", Name = "GetUserById")]
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

        [Authorize(Roles = "Admin")]
        [Route("user/{username}")]
        public async Task<IHttpActionResult> GetUserByName(string username)
        {
            try
            {
                var user = await AppUserManager.FindByNameAsync(username);
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

        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
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
                    JoinDate = user.JoinDate
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            try
            {
                Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            try
            {
                IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

                if (null == user)
                    return null;

                var logins = user.Logins.Select(linkedAccount => new UserLoginInfoViewModel
                    {
                        LoginProvider = linkedAccount.LoginProvider,
                        ProviderKey = linkedAccount.ProviderKey
                    })
                    .ToList();

                if (null != user.PasswordHash)
                    logins.Add(new UserLoginInfoViewModel
                    {
                        LoginProvider = LocalLoginProvider,
                        ProviderKey = user.UserName,
                    });

                return new ManageInfoViewModel
                {
                    LocalLoginProvider = LocalLoginProvider,
                    Email = user.UserName,
                    Logins = logins,
                    ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}")]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {
            //Only SuperAdmin or Admin can delete users (Later when implement roles)
            var appUser = await AppUserManager.FindByIdAsync(id);
            if (appUser == null) return NotFound();
            var result = await AppUserManager.DeleteAsync(appUser);
            return !result.Succeeded ? GetErrorResult(result) : Ok();
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                    model.NewPassword);

                return !result.Succeeded ? GetErrorResult(result) : Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                return !result.Succeeded ? GetErrorResult(result) : Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}/roles")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignRolesToUser([FromUri] string id, [FromBody] string[] rolesToAssign)
        {
            try
            {
                const string userRoleName = "User";
                var rolesToAssignList = new List<string>(rolesToAssign);
                rolesToAssignList.Remove(userRoleName);
                rolesToAssign = rolesToAssignList.ToArray();

                var appUser = await AppUserManager.FindByIdAsync(id);
                if (null == appUser)
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
                Logger.Error(ex);
                throw;
            }
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                var ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

                if (ticket?.Identity == null || ticket.Properties?.ExpiresUtc != null &&
                    ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow)
                    return BadRequest("External login failure.");

                var externalData = ExternalLoginData.FromIdentity(ticket.Identity);

                if (externalData == null)
                    return BadRequest("The external login is already associated with an account.");


                var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

                return !result.Succeeded ? GetErrorResult(result) : Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                IdentityResult result;

                if (model.LoginProvider == LocalLoginProvider)
                    result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
                else
                    result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                        new UserLoginInfo(model.LoginProvider, model.ProviderKey));

                return !result.Succeeded ? GetErrorResult(result) : Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            try
            {
                if (error != null)
                {
                    return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
                }

                if (!User.Identity.IsAuthenticated)
                {
                    return new ChallengeResult(provider, this);
                }

                var externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

                if (externalLogin == null)
                {
                    return InternalServerError();
                }

                if (externalLogin.LoginProvider != provider)
                {
                    Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                    return new ChallengeResult(provider, this);
                }

                var user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                    externalLogin.ProviderKey));

                var hasRegistered = user != null;

                if (hasRegistered)
                {
                    Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

                    var oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                        OAuthDefaults.AuthenticationType);
                    var cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                        CookieAuthenticationDefaults.AuthenticationType);

                    var properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                    Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
                }
                else
                {
                    IEnumerable<Claim> claims = externalLogin.GetClaims();
                    var identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                    Authentication.SignIn(identity);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            try
            {
                var descriptions = Authentication.GetExternalAuthenticationTypes();

                string state;

                if (generateState)
                {
                    const int strengthInBits = 256;
                    state = RandomOAuthStateGenerator.Generate(strengthInBits);
                }
                else
                    state = null;

                return descriptions.Select(description => new ExternalLoginViewModel
                    {
                        Name = description.Caption,
                        Url = Url.Route("ExternalLogin", new
                        {
                            provider = description.AuthenticationType,
                            response_type = "token",
                            client_id = Startup.PublicClientId,
                            redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                            state
                        }),
                        State = state
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var user = new ApplicationUser {UserName = model.Email, Email = model.Email};
                var result = await UserManager.CreateAsync(user, model.Password);
                return !result.Succeeded ? GetErrorResult(result) : Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var info = await Authentication.GetExternalLoginInfoAsync();
                if (null == info)
                    return InternalServerError();

                var user = new ApplicationUser {UserName = model.Email, Email = model.Email};

                var result = await UserManager.CreateAsync(user);
                if (!result.Succeeded)
                    return GetErrorResult(result);

                result = await UserManager.AddLoginAsync(user.Id, info.Login);
                return !result.Succeeded ? GetErrorResult(result) : Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && _userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }
                base.Dispose(disposing);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        #region Helpers

        private IAuthenticationManager Authentication => Request.GetOwinContext().Authentication;

        private new IHttpActionResult GetErrorResult(IdentityResult result)
        {
            try
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

        private static class RandomOAuthStateGenerator
        {
            private static readonly RandomNumberGenerator Random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                try
                {
                    const int bitsPerByte = 8;

                    if (strengthInBits % bitsPerByte != 0)
                        throw new ArgumentException("strengthInBits must be evenly divisible by 8.",
                            nameof(strengthInBits));
                    
                    var strengthInBytes = strengthInBits / bitsPerByte;

                    var data = new byte[strengthInBytes];
                    Random.GetBytes(data);
                    return HttpServerUtility.UrlTokenEncode(data);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    throw;
                }
            }
        }

        #endregion
    }
}
