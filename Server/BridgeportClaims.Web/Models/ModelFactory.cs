﻿using NLog;
using System;
using System.Net.Http;
using System.Web.Http.Routing;
using BridgeportClaims.Common.Constants;
using BridgeportClaims.Web.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BridgeportClaims.Web.Models
{   
    public sealed class ModelFactory
    {
        private readonly Lazy<UrlHelper> _urlHelper;
        private readonly ApplicationUserManager _appUserManager;
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);

        public ModelFactory(HttpRequestMessage request, ApplicationUserManager appUserManager)
        {
            _urlHelper = new Lazy<UrlHelper>(() => new UrlHelper(request));
            _appUserManager = appUserManager;
        }

        public UserReturnModel Create(ApplicationUser appUser)
        {
            try
            {
                return new UserReturnModel
                {
                    Url = _urlHelper.Value.Link(StringConstants.GetUserByIdAction, new {id = appUser.Id}),
                    Id = appUser.Id,
                    UserName = appUser.UserName,
                    FullName = $"{appUser.FirstName} {appUser.LastName}",
                    FirstName = appUser.FirstName,
                    LastName = appUser.LastName,
                    Deactivated = appUser.LockoutEnabled && appUser.LockoutEndDateUtc.HasValue && appUser.LockoutEndDateUtc.Value > DateTime.UtcNow,
                    Email = appUser.Email,
                    EmailConfirmed = appUser.EmailConfirmed,
                    RegisteredDate = appUser.RegisteredDate,
                    Extension = appUser.Extension,
                    ReferralTypeId = appUser.ReferralTypeId,
                    Roles = _appUserManager.GetRolesAsync(appUser.Id).GetAwaiter().GetResult(),
                    Claims = _appUserManager.GetClaimsAsync(appUser.Id).GetAwaiter().GetResult()
                };
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                throw;
            }
        }

        public RoleReturnModel Create(IdentityRole appRole)
        {
            try
            {
                return new RoleReturnModel
                {
                    Url = _urlHelper.Value.Link(StringConstants.GetRoleByIdAction, new { id = appRole.Id }),
                    Id = appRole.Id,
                    Name = appRole.Name
                };
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                throw;
            }
        }
    }
}