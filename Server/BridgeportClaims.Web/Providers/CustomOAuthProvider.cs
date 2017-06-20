using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using NLog;

namespace BridgeportClaims.Web.Providers
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            try
            {
                context.Validated();
                return Task.FromResult<object>(null);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                const string allowedOrigin = "*";
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] {allowedOrigin});

                var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

                var user = await userManager.FindAsync(context.UserName, context.Password);
                if (null == user)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
                if (!user.EmailConfirmed)
                {
                    context.SetError("invalid_grant", "User did not confirm email.");
                    return;
                }
                // Special validation that the user is a part of a role in order to be granted authorization to login.
                var hasAnyRoles = user.Roles?.Any();
                if (null == hasAnyRoles || !hasAnyRoles.Value)
                {
                    context.SetError("invalid_grant", $"User {user.UserName} is not yet a part of any roles. " +
                                                      "Please make this user at least a member of the \"User\" role group.");
                    return;
                }
                var oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, "JWT");
                var ticket = new AuthenticationTicket(oAuthIdentity, null);
                context.Validated(ticket);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}