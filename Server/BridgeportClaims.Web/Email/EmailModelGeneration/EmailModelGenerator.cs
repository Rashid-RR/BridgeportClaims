using System;
using System.Net;
using System.Net.Mail;
using System.Management.Instrumentation;
using BridgeportClaims.Web.Email.EmailTemplateProviders;
using BridgeportClaims.Web.Email.Models;
using cs = BridgeportClaims.Common.Config.ConfigService;
using c = BridgeportClaims.Common.StringConstants.Constants;

namespace BridgeportClaims.Web.Email.EmailModelGeneration
{
    public class EmailModelGenerator : IEmailModelGenerator
    {
        public EmailModel GenerateEmailModelFromTemplate<TTemplate>(EmailViewModel model)
            where TTemplate : IEmailTemplateProvider, new()
        {
            var subject = string.Empty;
            switch (model.EmailModelEnum)
            {
                case EmailModelEnum.PasswordReset:
                    subject = c.PasswordResetTemplateEmailSubject;
                    break;
                case EmailModelEnum.WelcomeActivation:
                    subject = c.EmailWelcomeActivationTemplateEmailSubject;
                    break;
                case EmailModelEnum.LakerImportStatus:
                    subject = c.LakerImportStatus;
                    break;
                case EmailModelEnum.Unknown:
                    break;
                default:
                    throw new InstanceNotFoundException("Cannot determine the emailEnum enum type.");
            }

            var template = new TTemplate();
            var sourceEmailAddress = cs.GetAppSetting("SourceEmailAddress");
            var sourceEmailPassword = cs.GetAppSetting("EmailPassword");
            var emailModel = new EmailModel
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EmailCredential = new NetworkCredential(sourceEmailAddress, sourceEmailPassword),
                EmailHost = cs.GetAppSetting("EmailHost"),
                SourceEmailDisplayName = cs.GetAppSetting("SourceEmailDisplayName"),
                EmailPort = Convert.ToInt32(cs.GetAppSetting("EmailPort")),
                UseDefaultCredential = false,
                EnableSsl = true,
                IsBodyHtml = true,
                EmailSubject = subject,
                EmailBody = template.GetTemplatedEmailBody(model),
                SourceEmailAddress = sourceEmailAddress
            };
            return emailModel;
        }
    }
}