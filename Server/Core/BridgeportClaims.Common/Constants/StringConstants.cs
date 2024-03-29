﻿namespace BridgeportClaims.Common.Constants
{
	public static class StringConstants
	{
		public const string OtherImportFileTypeCode = "OT";
		public const string LakerImportImportFileTypeCode = "LI";
		public const string PaymentImportFileTypeCode = "PI";
        public const string EnvisionImportImportFileTypeCode = "EI";
        public const string DbConnStrName = "BridgeportClaimsConnectionString";
		public const string AppIsInDebugMode = "ApplicationIsInDebugMode";
		public const string GetUserByIdAction = "GetUserById";
		public const string EmailWelcomeActivationTemplateEmailSubject =
			"Thank you for Registering to BridgeportClaims.com, Please Activate your Email Address";
        public const string EnvisionImportStatus = "The Envision Import Process has Completed.";
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
	    public const string UdtId = "[dbo].[udtID]";
		public const string Comma = ",";
		public const string CachingProvider = "CachingProvider";
		public const string LakeFileNameStartsWithString = "Billing_Claim_File_";
		public const string NoLakerFilesToImportToast = "No Laker Files were found to import.";
        public const string NoEnvisionFilesFound = "No Envision File was found to process.";
		public const string LakerImportStatus = "The Laker Import Process has Completed.";
		public const string SecureDbConnStrName = "BridgeportClaimsSecureConnectionString";
		public const string ImeLetterName = "IME Letter.docx";
	    public const string DenialLetterName = "Denial Letter.docx";
        public const string LienRequestLetterName = "Lien Request.docx";
	    public const string DrLetterName = "Dr Note Request.docx";
        public const string NcofLetterName = "NCOF Letter.docx";
        public const string BenefitsExhaustedLetter = "Benefits Exhausted Letter.docx";
        public const string DeductibleLetterName = "Deductible Letter.docx";
		public const string PipAppLetter = "PIP App Letter.docx";
	    public const string UnderInvestigationLetterName = "Under Investigation.docx";
        public const string WordDocumentManifestPrefix = "BridgeportClaims.Word.EmbeddedResources.";
	    public const string UnderInvestigationManifestResource = WordDocumentManifestPrefix + UnderInvestigationLetterName;
        public const string ImeLetterManifestResource = WordDocumentManifestPrefix + ImeLetterName;
	    public const string BenefitsExhaustedLetterManifestResource = WordDocumentManifestPrefix + BenefitsExhaustedLetter;
        public const string DenialLetterManifestResource = WordDocumentManifestPrefix + DenialLetterName;
	    public const string PipAppLetterManifestResource = WordDocumentManifestPrefix + PipAppLetter;
	    public const string DrNoteLetterManifestResource = WordDocumentManifestPrefix + DrLetterName;
        public const string NcofLetterManifestResource = WordDocumentManifestPrefix + NcofLetterName;
        public const string DeductibleLetterManifestResource = WordDocumentManifestPrefix + DeductibleLetterName;
        public const string LienRequestLetterManifestResource = WordDocumentManifestPrefix + LienRequestLetterName;
        public const string ContentDispositionUniqueString = "inline; filename=";
	    public const string ExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
	    public const string BillingStatementName = "Billing Statement";
	    public const string RedisCacheConnection = "CacheConnection";
	    public const string UseRedisKey = "UseRedis";
	    public const string IsProductionKey = "IsProduction";
	    public const string TimeFormat = "M/d/yyyy h:mm:ss tt";
	    public const string ApplicationJson = "application/json";
        public const string PdfApiUrlKey = "PdfApiUrlKey";
        public const string PdfApiUrlPath = "PdfApiUrlPath";
    }
}