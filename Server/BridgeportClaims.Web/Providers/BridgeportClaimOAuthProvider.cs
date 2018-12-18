using NLog;
using System;
using System.Threading.Tasks;
using BridgeportClaims.Web.Infrastructure;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Linq;

namespace BridgeportClaims.Web.Providers
{
    public class BridgeportClaimOAuthProvider : OAuthAuthorizationServerProvider
    {
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);
        private const string InvalidGrant = "invalid_grant";

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext ctx)
        {
            ctx.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext ctx)
        {
            try
            {
                var userManager = ctx.OwinContext.GetUserManager<ApplicationUserManager>();
                var user = await userManager.FindAsync(ctx.UserName, ctx.Password);
                if (null == user)
                {
                    ctx.SetError(InvalidGrant, "The user name or password is incorrect.");
                    return;
                }
                if (!user.EmailConfirmed)
                {
                    ctx.SetError(InvalidGrant, "User did not confirm email.");
                    return;
                }
                // Special validation that the user is a part of a role in order to be granted authorization to login.
                var hasAnyRoles = user.Roles?.Any();
                if (null == hasAnyRoles || !hasAnyRoles.Value)
                {
                    ctx.SetError(InvalidGrant, $"User {user.UserName} is not yet a part of any roles. " +
                                                      "Please make this user at least a member of the \"User\" role group.");
                    return;
                }
                if (user.LockoutEnabled && user.LockoutEndDateUtc.HasValue && DateTime.UtcNow < user.LockoutEndDateUtc)
                {
                    ctx.SetError(InvalidGrant, $"User {user.UserName} has been deactivated from the system. A user with the" +
                                                   " Admin role may activate them on the 'Manage Users' page.");
                    return;
                }
                var oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, "JWT");
                var ticket = new AuthenticationTicket(oAuthIdentity, null);
                ctx.Validated(ticket);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                throw;
            }
        }
    }
}