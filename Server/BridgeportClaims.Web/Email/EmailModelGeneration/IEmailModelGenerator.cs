using BridgeportClaims.Web.Email.EmailTemplateProviders;
using BridgeportClaims.Web.Email.Models;
using BridgeportClaims.Web.Models;

namespace BridgeportClaims.Web.Email.EmailModelGeneration
{
    public interface IEmailModelGenerator
    {
        EmailModel GenerateEmailModelFromTemplate<TTemplate>(ConfirmRegistrationModel model)
            where TTemplate : IEmailTemplateProvider, new();
    }
}