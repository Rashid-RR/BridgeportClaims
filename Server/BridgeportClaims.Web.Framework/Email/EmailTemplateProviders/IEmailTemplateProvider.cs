namespace BridgeportClaims.Web.Framework.Email.EmailTemplateProviders
{
    public interface IEmailTemplateProvider
    {
        string GetTemplatedEmailBody<TModel>(TModel model) where TModel : class, new();
    }
}