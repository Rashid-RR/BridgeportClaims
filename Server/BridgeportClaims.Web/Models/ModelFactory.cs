using System;
using System.Net.Http;
using System.Web.Http.Routing;
using Microsoft.AspNet.Identity.EntityFramework;
using NLog;

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

        public RoleReturnModel Create(IdentityRole appRole)
        {
            try
            {
                return new RoleReturnModel
                {
                    Url = _urlHelper.Link("GetRoleById", new {id = appRole.Id}),
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

        public UserReturnModel Create(ApplicationUser appUser)
        {
            try
            {
                return new UserReturnModel
                {
                    Url = _urlHelper.Link("GetUserById", new {id = appUser.Id}),
                    Id = appUser.Id,
                    UserName = appUser.UserName,
                    FirstName = appUser.FirstName,
                    LastName = appUser.LastName,
                    FullName = $"{appUser.FirstName} {appUser.LastName}",
                    Email = appUser.Email,
                    EmailConfirmed = appUser.EmailConfirmed,
                    JoinDate = appUser.JoinDate,
                    Roles = _appUserManager.GetRolesAsync(appUser.Id).Result,
                    Claims = _appUserManager.GetClaimsAsync(appUser.Id).Result
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