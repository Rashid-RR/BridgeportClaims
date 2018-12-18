using System;
using BridgeportClaims.Common.Constants;
using BridgeportClaims.Web.Email;
using BridgeportClaims.Web.Email.EmailModelGeneration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace BridgeportClaims.Web.Infrastructure
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store) { }

        public static ApplicationUserManager Create(IdentityFactoryOptions<
            ApplicationUserManager> options, IOwinContext context)
        {
            var appDbContext = context.Get<ApplicationDbContext>();
            var appUserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(appDbContext))
            {
                EmailService = new EmailService(new EmailModelGenerator()),
                UserLockoutEnabledByDefault = true,
                MaxFailedAccessAttemptsBeforeLockout = 5,
                DefaultAccountLockoutTimeSpan = TimeSpan.FromDays(365 * 500)
            };
            // 500 years

            //Rest of code is removed for clarity

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                appUserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(
                    dataProtectionProvider.Create(StringConstants.AspNetIdentity))
                {
                    //Code for email confirmation and reset password life time
                    TokenLifespan = TimeSpan.FromHours(24)
                };
            }
            appUserManager.UserValidator = new UserValidator<ApplicationUser>(appUserManager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            appUserManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            return appUserManager;
        }
    }
}