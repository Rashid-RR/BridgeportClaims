using BridgeportClaims.Web.Email.EmailTemplateProviders;
using BridgeportClaims.Web.Email.Model;

namespace BridgeportClaims.Web.Email.EmailModelGeneration
{
    public interface IEmailModelGenerator
    {
        EmailModel GenerateEmailModelFromTemplate<TTemplate>(string baseUrl) where TTemplate : IEmailTemplateProvider, new();
    }
}