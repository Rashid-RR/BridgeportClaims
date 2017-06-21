using System;
using System.Net;
using System.Net.Mail;
using BridgeportClaims.Common.Config;
using BridgeportClaims.Web.Email.EmailTemplateProviders;
using BridgeportClaims.Web.Email.Models;
using BridgeportClaims.Web.Models;
using c = BridgeportClaims.Common.StringConstants.Constants;

namespace BridgeportClaims.Web.Email.EmailModelGeneration
{
    public class EmailModelGenerator : IEmailModelGenerator
    {
        public EmailModel GenerateEmailModelFromTemplate<TTemplate>(ConfirmRegistrationModel model)
            where TTemplate : IEmailTemplateProvider, new()
        {
            var template = new TTemplate();
            var sourceEmailAddress = ConfigService.GetAppSetting("SourceEmailAddress");
            var sourceEmailPassword = ConfigService.GetAppSetting("EmailPassword");
            var emailModel = new EmailModel
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EmailCredential = new NetworkCredential(sourceEmailAddress, sourceEmailPassword),
                EmailHost = ConfigService.GetAppSetting("EmailHost"),
                SourceEmailDisplayName = ConfigService.GetAppSetting("SourceEmailDisplayName"),
                EmailPort = Convert.ToInt32(ConfigService.GetAppSetting("EmailPort")),
                UseDefaultCredential = false,
                EnableSsl = true,
                IsBodyHtml = true,
                EmailSubject = c.EmailWelcomeActivationTemplateEmailSubject,
                EmailBody = template.GetTemplatedEmailBody(model),
                SourceEmailAddress = sourceEmailAddress
            };
            return emailModel;
        }
    }
}