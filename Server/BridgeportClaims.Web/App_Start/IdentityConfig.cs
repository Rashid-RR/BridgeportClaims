using System;
using System.Web.Http;
using BridgeportClaims.Services.Constants;
using BridgeportClaims.Web.Email;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using BridgeportClaims.Web.Models;

namespace BridgeportClaims.Web
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store) { }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var dependencyResolver = GlobalConfiguration.Configuration.DependencyResolver;
            var constantsService = dependencyResolver.GetService(typeof(IConstantsService)) as IConstantsService;
            var emailService = dependencyResolver.GetService(typeof(IEmailService)) as IEmailService;
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()))
                {
                    UserLockoutEnabledByDefault = true,
                    MaxFailedAccessAttemptsBeforeLockout = 6,
                    EmailService = emailService
                };
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = true,
                RequireUppercase = true
            };
            var dataProtectionProvider = options.DataProtectionProvider;
            if (null != dataProtectionProvider)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create(constantsService?.DataSecurityProtection))
                    {
                        //Code for email confirmation and reset password life time
                        TokenLifespan = TimeSpan.FromHours(24)
                    };
            }
            return manager;
        }
    }
}
