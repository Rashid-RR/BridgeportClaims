using System.Configuration;

namespace BridgeportClaims.Common.StringConstants
{
    public static class Constants
    {
        public const string DbConnStrName = "BridgeportClaimsConnectionString";
        public const string AppIsInDebugMode = "ApplicationIsInDebugMode";
        public const string GetUserByIdAction = "GetUserById";
        public const string ThankYouEmailConfirmedAction = "ThankYouEmailConfirmed";
        public const string EmailWelcomeActivationTemplateEmailSubject = 
            "Thank you for Registering to BridgeportClaims.com, Please Activate your Email Address";
        public const string PasswordResetTemplateEmailSubject =
            "Forgotten Password Assistance from BridgeportClaims.com";
        public const string AspNetIdentity = "ASP.NET Identity";
        public const string ConfirmEmailRouteAction = "ConfirmEmailRoute";
        public const string GetRoleByIdAction = "GetRoleById";
        public const string ResetPasswordRouteAction = "ResetPasswordRoute";
        public const string GetAllRolesAction = "GetAllRoles";
        public const string ManageUsersInRoleAction = "ManageUsersInRole";
        public const string GetClaimNoteAction = "GetClaimNote";
        public static string ConnStr = ConfigurationManager.ConnectionStrings[DbConnStrName].ConnectionString;
        public const string ClientLocalHostNameKey = "ClientLocalHostName";
        public const string ServerLocalHostNameKey = "ServerLocalHostName";
        public const string ResetPasswordClientRoute = "resetpassword";
        public const string ConfirmEmailClientRoute = "confirm-email";
        public const string SecureServerLocalHostNameKey = "SecureServerLocalHostName";
    }
}