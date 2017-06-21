namespace BridgeportClaims.Web.Email.EmailTemplateProviders
{
    public interface IEmailTemplateProvider
    {
        string GetTemplatedEmailBody<TModel>(TModel model) where TModel : class, new();
    }
}