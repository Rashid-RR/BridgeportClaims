using NLog;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using BridgeportClaims.Web.Email.EmailModelGeneration;
using BridgeportClaims.Web.Email.EmailTemplateProviders;
using BridgeportClaims.Web.Email.Models;
using BridgeportClaims.Web.EmailTemplates;
using Microsoft.AspNet.Identity;

namespace BridgeportClaims.Web.Email
{
    public class EmailService : IEmailService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IEmailModelGenerator _emailModelGenerator;

        public EmailService(IEmailModelGenerator emailModelGenerator)
        {
            _emailModelGenerator = emailModelGenerator;
        }

        public async Task SendEmail<TTemplate>(IList<string> destinationEmailAddresses, 
            string fullName, string url, EmailModelEnum emailModelEnum)
            where TTemplate : IEmailTemplateProvider, new()
        {
            var msg = new MailMessage();
            foreach(var addressee in destinationEmailAddresses)
                msg.To.Add(new MailAddress(addressee));
            var model = new EmailViewModel
            {
                FullName = fullName,
                Uri = url,
                EmailModelEnum = emailModelEnum
            };
            await SendEmail<TTemplate>(msg, model);
        }

        public async Task SendEmail<TTemplate>(string destinationEmailAddress, 
            string fullName, string url, EmailModelEnum emailModelEnum)
            where TTemplate : IEmailTemplateProvider, new()
        {
            var msg = new MailMessage();
            msg.To.Add(new MailAddress(destinationEmailAddress));
            var model = new EmailViewModel
            {
                FullName = fullName,
                Uri = url,
                EmailModelEnum = emailModelEnum
            };
            await SendEmail<TTemplate>(msg, model);
        }

        private async Task SendEmail<TTemplate>(MailMessage msg, EmailViewModel emailViewModel) 
            where TTemplate : IEmailTemplateProvider, new()
        {
            var model = _emailModelGenerator.GenerateEmailModelFromTemplate<TTemplate>(emailViewModel);
            var sourceEmailAddress = model.SourceEmailAddress;

            var client = new SmtpClient
            {
                DeliveryMethod = model.DeliveryMethod,
                EnableSsl = model.EnableSsl,
                Host = model.EmailHost,
                Port = model.EmailPort
            };
            var credentials = model.EmailCredential;
            client.UseDefaultCredentials = model.UseDefaultCredential;
            client.Credentials = credentials;
            msg.From = new MailAddress(sourceEmailAddress, model.SourceEmailDisplayName);
            msg.Subject = model.EmailSubject;
            msg.IsBodyHtml = model.IsBodyHtml;
            msg.Body = model.EmailBody;
            try
            {
                await client.SendMailAsync(msg);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "SmtpClient Send() method");
                throw;
            }
        }

        public async Task SendAsync(IdentityMessage message)
        {
            // This is so wrong. We're using the Full Name for the Email Subject, and the Absolute Activation Uri for the Email body.
            const string resetPassword = "resetpassword";
            if (null != message?.Body?.ToLower().Contains(resetPassword) &&
                message.Body.ToLower().Contains(resetPassword))
            {
                await SendEmail<EmailTemplateProvider>(message.Destination, message.Subject, message.Body, EmailModelEnum.PasswordReset);
            }
            else
            {
                // ReSharper disable once PossibleNullReferenceException
                await SendEmail<EmailTemplateProvider>(message.Destination, message.Subject, message.Body, EmailModelEnum.WelcomeActivation);
            }
        }
    }
}
