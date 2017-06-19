using System;
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