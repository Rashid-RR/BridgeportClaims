using System;
using System.Linq;
using System.Threading.Tasks;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;

namespace BridgeportClaims.Web.Validators
{
    public class CustomRoleUserValidator : UserValidator<ApplicationUser>
    {
        #region Ctor
        public CustomRoleUserValidator(UserManager<ApplicationUser, string> manager) : base(manager) { }
        #endregion

        #region Overridden Methods
        public override async Task<IdentityResult> ValidateAsync(ApplicationUser user)
        {
            if (null == user)
                throw new ArgumentNullException(nameof(user));
            var result = await base.ValidateAsync(user);
            var hasAnyRoles = user.Roles?.Any();
            if (null != hasAnyRoles && hasAnyRoles.Value)
                return result;
            var errors = result.Errors.ToList();
            errors.Add($"The user {user.UserName} must be in a Role to Login.");
            result = new IdentityResult(errors);
            return result;
        }
        #endregion
    }
}