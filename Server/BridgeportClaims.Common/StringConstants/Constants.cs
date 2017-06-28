﻿using System.Configuration;

namespace BridgeportClaims.Common.StringConstants
{
    public static class Constants
    {
        public const string DbConnStrName = "BridgeportClaimsConnectionString";
        public const string AppIsInDebugMode = "ApplicationIsInDebugMode";
        public const string GetUserByIdAction = "GetUserById";
        public const string EmailWelcomeActivationTemplateEmailSubject = 
            "Thank you for Registering to BridgeportClaims.com, Please Activate your Email Address";
        public const string AspNetIdentity = "ASP.NET Identity";
        public const string ConfirmEmailRouteAction = "ConfirmEmailRoute";
        public const string GetRoleByIdAction = "GetRoleById";
        public const string GetAllRolesAction = "GetAllRoles";
        public const string ManageUsersInRoleAction = "ManageUsersInRole";
        public static string ConnStr = ConfigurationManager.ConnectionStrings[DbConnStrName].ConnectionString;
    }
}