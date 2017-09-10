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
        public const string ClientLocalHostNameKey = "ClientLocalHostName";
        public const string ServerLocalHostNameKey = "ServerLocalHostName";
        public const string ResetPasswordClientRoute = "resetpassword";
        public const string ConfirmEmailClientRoute = "confirm-email";
        public const string SecureServerLocalHostNameKey = "SecureServerLocalHostName";
        public const string MaxClaimsLookupHistoryItemsKey = "MaxClaimsLookupHistoryItems";
        public const string DateDisplayCacheKey = "__DateDisplayCacheKey__";
        public const string PrescriptionNoteTypesKey = "__PrescriptionNoteTypes__";
        public const string ImportFileDatabaseCachingKey = "__ImportFile__";
        public const string ClaimNoteTypesKey = "__ClaimNoteTypesKey__";
        public const string TestClaimIdKey = "TestClaimId";
        public const string TestUserIdKey = "TestUserId";
        public const string PaymentsDataImporterAppName = "PaymentsDataImporter";
        public const string LakerFilePatternKeyName = "LakerFilePattern";
        public const string PaymentFilePatternKeyName = "PaymentFilePattern";
        public const string Comma = ",";
        public const string GetAllUsersCacheKey = "__GetAllUsersCacheKey__";
        public const string EpisodeTypesCacheKey = "__EpisodeTypesCacheKey__";
        public const string PaymentChannel = "PaymentChannel";
        public const string PrescriptionBladeCacheKey = "__PrescriptionBladeCacheKey__";
        public const string CachingProvider = "CachingProvider";
        public const string LakeFileNameStartsWithString = "Billing_Claim_File_";
        public const string NoLakerFilesToImportToast = "No Laker Files were found to import.";
        public const string LakerImportStatus = "The Laker Import Process has Completed.";
    }
}