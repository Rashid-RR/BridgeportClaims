using BridgeportClaims.Web.Email.EmailTemplateProviders;
using BridgeportClaims.Web.Email.Models;

namespace BridgeportClaims.Web.Email.EmailModelGeneration
{
    public interface IEmailModelGenerator
    {
        EmailModel GenerateEmailModelFromTemplate<TTemplate>(EmailViewModel model)
            where TTemplate : IEmailTemplateProvider, new();
    }
}