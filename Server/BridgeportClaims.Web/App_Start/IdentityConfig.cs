using System;
using BridgeportClaims.Services.Config;
using BridgeportClaims.Services.Constants;
using BridgeportClaims.Web.Email;
using BridgeportClaims.Web.Email.EmailModelGeneration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using BridgeportClaims.Web.Models;
using BridgeportClaims.Web.Security;
using Microsoft.Owin.Security.DataProtection;

namespace BridgeportClaims.Web
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store) { }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, 
            IOwinContext context)
        {
            var constantsService = new ConstantsService();
            var emailService = new EmailService(new EmailModelGenerator(new ConfigService(), new ConstantsService()));
            if (null == emailService)
                throw new ArgumentNullException(nameof(emailService));
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
            IDataProtectionProvider dataProtectionProvider = Startup.DataProtectionProvider;
            if (null == dataProtectionProvider)
                throw new ArgumentNullException(nameof(dataProtectionProvider));
            //var dataProtectionProvider = options.DataProtectionProvider;
            manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(
                dataProtectionProvider.Create(constantsService.DataSecurityProtection))
                {
                    //Code for email confirmation and reset password life time
                    TokenLifespan = TimeSpan.FromHours(24)
                };
            return manager;
        }
    }
}
