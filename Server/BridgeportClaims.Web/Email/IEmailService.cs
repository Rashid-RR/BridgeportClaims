using System.Collections.Generic;
using System.Threading.Tasks;
using BridgeportClaims.Web.Email.EmailTemplateProviders;

namespace BridgeportClaims.Web.Email
{
    public interface IEmailService
    {
        Task SendEmail<TTemplate>(IList<string> destinationEmailAddresses, string baseUrl) where TTemplate : IEmailTemplateProvider, new();
        Task SendEmail<TTemplate>(string destinationEmailAddress, string baseUrl) where TTemplate : IEmailTemplateProvider, new();
    }
}