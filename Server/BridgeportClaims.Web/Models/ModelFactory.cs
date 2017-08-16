using NLog;
using System;
using System.Net.Http;
using System.Web.Http.Routing;
using BridgeportClaims.Web.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;
using c = BridgeportClaims.Common.StringConstants.Constants;

namespace BridgeportClaims.Web.Models
{   
    public class ModelFactory
    {
        private readonly UrlHelper _urlHelper;
        private readonly ApplicationUserManager _appUserManager;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ModelFactory(HttpRequestMessage request, ApplicationUserManager appUserManager)
        {
            _urlHelper = new UrlHelper(request);
            _appUserManager = appUserManager;
        }

        public UserReturnModel Create(ApplicationUser appUser)
        {
            try
            {
                return new UserReturnModel
                {
                    Url = _urlHelper.Link(c.GetUserByIdAction, new {id = appUser.Id}),
                    Id = appUser.Id,
                    UserName = appUser.UserName,
                    FullName = $"{appUser.FirstName} {appUser.LastName}",
                    FirstName = appUser.FirstName,
                    LastName = appUser.LastName,
                    Deactivated = appUser.LockoutEnabled && appUser.LockoutEndDateUtc.HasValue && appUser.LockoutEndDateUtc.Value > DateTime.UtcNow,
                    Email = appUser.Email,
                    EmailConfirmed = appUser.EmailConfirmed,
                    RegisteredDate = appUser.RegisteredDate,
                    Roles = _appUserManager.GetRolesAsync(appUser.Id).GetAwaiter().GetResult(),
                    Claims = _appUserManager.GetClaimsAsync(appUser.Id).GetAwaiter().GetResult()
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        public RoleReturnModel Create(IdentityRole appRole)
        {
            try
            {
                return new RoleReturnModel
                {
                    Url = _urlHelper.Link(c.GetRoleByIdAction, new { id = appRole.Id }),
                    Id = appRole.Id,
                    Name = appRole.Name
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