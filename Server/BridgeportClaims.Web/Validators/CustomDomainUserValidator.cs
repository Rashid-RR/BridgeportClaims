using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BridgeportClaims.Web.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;

namespace BridgeportClaims.Web.Validators
{
    public class CustomDomainUserValidator : UserValidator<ApplicationUser>
    {
        public CustomDomainUserValidator(UserManager<ApplicationUser, string> manager) : base(manager) { }

        private readonly IList<string> _allowedEmailDomains = new List<string> { "bridgeportclaims.com" };

        public override async Task<IdentityResult> ValidateAsync(ApplicationUser user)
        {
            var result = await base.ValidateAsync(user);
            var emailDomain = user.UserName.Split('@')[1];
            if (_allowedEmailDomains.Contains(emailDomain)) return result;
            var errors = result.Errors.ToList();
            errors.Add($"Email domain {emailDomain} is not allowed.");
            result = new IdentityResult(errors);
            return result;
        }
    }
}