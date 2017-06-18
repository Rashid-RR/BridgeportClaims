using BridgeportClaims.Web.Email.EmailTemplateProviders;
using BridgeportClaims.Web.Email.Model;
using BridgeportClaims.Web.Models;

namespace BridgeportClaims.Web.Email.EmailModelGeneration
{
    public interface IEmailModelGenerator
    {
        EmailModel GenerateEmailModelFromTemplate<TTemplate>(ActivationEmailModel model)
            where TTemplate : IEmailTemplateProvider, new();
    }
}