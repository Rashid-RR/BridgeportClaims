namespace BridgeportClaims.Services.Constants
{
    public class ConstantsService : IConstantsService
    {
        private const string BridgeportClaimsDatabaseConnectionString = "BridgeportClaimsConnectionString";
        private const string DataSecurityProtectionString = "ASP.NET Identity";
        private const string EmailWelcomeActivationTemplateEmailSubjectString = "Thank you for Registering to BridgeportClaims.com, Please Activate your Email Address";
        public string DbConnStr => BridgeportClaimsDatabaseConnectionString;
        public string DataSecurityProtection => DataSecurityProtectionString;
        public string EmailWelcomeActivationTemplateEmailSubject => EmailWelcomeActivationTemplateEmailSubjectString;
    }
}