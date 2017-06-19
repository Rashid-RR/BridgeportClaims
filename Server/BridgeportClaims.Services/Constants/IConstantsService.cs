namespace BridgeportClaims.Services.Constants
{
    public interface IConstantsService
    {
        string DbConnStr { get; }
        string DataSecurityProtection { get; }
        string EmailWelcomeActivationTemplateEmailSubject { get; }
    }
}