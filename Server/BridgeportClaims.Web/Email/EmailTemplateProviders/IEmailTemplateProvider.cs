namespace BridgeportClaims.Web.Email.EmailTemplateProviders
{
    public interface IEmailTemplateProvider
    {
        string EmailSubject { get; }
        string GetTemplatedEmailBody(string baseUrl);
    }
}