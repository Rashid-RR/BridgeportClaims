using BridgeportClaims.Web.Framework.Email.EmailTemplateProviders;
using BridgeportClaims.Web.Framework.Models;

namespace BridgeportClaims.Web.Framework.Email.EmailModelGeneration
{
    public interface IEmailModelGenerator
    {
        EmailModel GenerateEmailModelFromTemplate<TTemplate>(EmailViewModel model)
            where TTemplate : IEmailTemplateProvider, new();
    }
}