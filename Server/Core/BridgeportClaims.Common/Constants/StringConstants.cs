namespace BridgeportClaims.Common.Constants
{
	public static class StringConstants
	{
		public const string OtherImportFileTypeCode = "OT";
		public const string LakerImportImportFileTypeCode = "LI";
		public const string PaymentImportFileTypeCode = "PI";
		public const string DbConnStrName = "BridgeportClaimsConnectionString";
		public const string AppIsInDebugMode = "ApplicationIsInDebugMode";
		public const string GetUserByIdAction = "GetUserById";
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
		public const string ClientLocalHostNameKey = "ClientLocalHostName";
		public const string ServerLocalHostNameKey = "ServerLocalHostName";
		public const string ResetPasswordClientRoute = "resetpassword";
		public const string ConfirmEmailClientRoute = "confirm-email";
		public const string SecureServerLocalHostNameKey = "SecureServerLocalHostName";
		public const string MaxClaimsLookupHistoryItemsKey = "MaxClaimsLookupHistoryItems";
		public const string udtId = "[dbo].[udtID]";
		public const string Comma = ",";
		public const string CachingProvider = "CachingProvider";
		public const string LakeFileNameStartsWithString = "Billing_Claim_File_";
		public const string NoLakerFilesToImportToast = "No Laker Files were found to import.";
		public const string LakerImportStatus = "The Laker Import Process has Completed.";
		public const string SecureDbConnStrName = "BridgeportClaimsSecureConnectionString";
		public const string ImeLetterName = "IME Letter.docx";
		public const string BenefitsExhaustedLetter = "Benefits Exhausted Letter.docx";
		public const string PipAppLetter = "PIP App Letter.docx";
		public const string ImeLetterManifestResource = "BridgeportClaims.Word.EmbeddedResources.IME Letter.docx";
		public const string BenefitsExhaustedLetterManifestResource = "BridgeportClaims.Word.EmbeddedResources.Benefits Exhausted Letter.docx";
		public const string PipAppLetterManifestResource = "BridgeportClaims.Word.EmbeddedResources.PIP App Letter.docx";
	    public const string ContentDispositionUniqueString = "inline; filename=";
	    public const string ExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
	    public const string BillingStatementName = "Billing Statement";
	    public const string RedisCacheConnection = "CacheConnection";
	    public const string UseRedisKey = "UseRedis";
	}
}