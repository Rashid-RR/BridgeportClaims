using System;
using System.Net;
using System.Net.Mail;
using BridgeportClaims.Services.Config;
using BridgeportClaims.Web.Email.EmailTemplateProviders;
using BridgeportClaims.Web.Email.Model;

namespace BridgeportClaims.Web.Email.EmailModelGeneration
{
    public class EmailModelGenerator : IEmailModelGenerator
    {
        private readonly IConfigService _configService;

        public EmailModelGenerator(IConfigService configService)
        {
            _configService = configService;
        }

        public EmailModel GenerateEmailModelFromTemplate<TTemplate>(string baseUrl) where TTemplate : IEmailTemplateProvider, new()
        {
            var template = new TTemplate();
            var sourceEmailAddress = _configService.GetConfigItem("SourceEmailAddress");
            var sourceEmailPassword = _configService.GetConfigItem("EmailPassword");
            var emailModel = new EmailModel
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EmailCredential = new NetworkCredential(sourceEmailAddress, sourceEmailPassword),
                EmailHost = _configService.GetConfigItem("EmailHost"),
                EmailSubject = template.EmailSubject,
                SourceEmailDisplayName = _configService.GetConfigItem("SourceEmailDisplayName"),
                EmailPort = Convert.ToInt32(_configService.GetConfigItem("EmailPort")),
                BaseUrl = baseUrl,
                UseDefaultCredential = false,
                EnableSsl = true,
                IsBodyHtml = true,
                EmailBody = template.GetTemplatedEmailBody(baseUrl),
                SourceEmailAddress = sourceEmailAddress
            };
            return emailModel;
        }
    }
}