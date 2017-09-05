using System.Collections.Generic;
using System.Threading.Tasks;
using BridgeportClaims.Web.Email.EmailModelGeneration;
using BridgeportClaims.Web.Email.EmailTemplateProviders;
using Microsoft.AspNet.Identity;

namespace BridgeportClaims.Web.EmailTemplates
{
    public interface IEmailService : IIdentityMessageService
    {
        Task SendEmail<TTemplate>(IList<string> destinationEmailAddresses, string baseUrl, 
            string url, EmailModelEnum emailModelEnum) where TTemplate : IEmailTemplateProvider, new();
        Task SendEmail<TTemplate>(string destinationEmailAddress, string baseUrl, 
            string url, EmailModelEnum emailModelEnum) where TTemplate : IEmailTemplateProvider, new();
    }
}