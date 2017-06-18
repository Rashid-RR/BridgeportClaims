using System;
using System.Net;
using System.Net.Mail;
using BridgeportClaims.Services.Config;
using BridgeportClaims.Services.Constants;
using BridgeportClaims.Web.Email.EmailTemplateProviders;
using BridgeportClaims.Web.Email.Model;
using BridgeportClaims.Web.Models;

namespace BridgeportClaims.Web.Email.EmailModelGeneration
{
    public class EmailModelGenerator : IEmailModelGenerator
    {
        private readonly IConfigService _configService;
        private readonly IConstantsService _constantsService;

        public EmailModelGenerator(IConfigService configService, IConstantsService constantsService)
        {
            _configService = configService;
            _constantsService = constantsService;
        }

        public EmailModel GenerateEmailModelFromTemplate<TTemplate>(ActivationEmailModel model)
            where TTemplate : IEmailTemplateProvider, new()
        {
            var template = new TTemplate();
            var sourceEmailAddress = _configService.GetConfigItem("SourceEmailAddress");
            var sourceEmailPassword = _configService.GetConfigItem("EmailPassword");
            var emailModel = new EmailModel
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EmailCredential = new NetworkCredential(sourceEmailAddress, sourceEmailPassword),
                EmailHost = _configService.GetConfigItem("EmailHost"),
                SourceEmailDisplayName = _configService.GetConfigItem("SourceEmailDisplayName"),
                EmailPort = Convert.ToInt32(_configService.GetConfigItem("EmailPort")),
                UseDefaultCredential = false,
                EnableSsl = true,
                IsBodyHtml = true,
                EmailSubject = _constantsService.EmailWelcomeActivationTemplateEmailSubject,
                EmailBody = template.GetTemplatedEmailBody(model),
                SourceEmailAddress = sourceEmailAddress
            };
            return emailModel;
        }
    }
}