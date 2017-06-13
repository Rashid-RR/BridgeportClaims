using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using BridgeportClaims.Web.Email.EmailModelGeneration;
using BridgeportClaims.Web.Email.EmailTemplateProviders;
using NLog;

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

        public async Task SendEmail<TTemplate>(IList<string> destinationEmailAddresses, string baseUrl) where TTemplate : IEmailTemplateProvider, new()
        {
            var msg = new MailMessage();
            foreach(var addressee in destinationEmailAddresses)
                msg.To.Add(new MailAddress(addressee));
            await SendEmail<TTemplate>(msg, baseUrl);
        }

        public async Task SendEmail<TTemplate>(string destinationEmailAddress, string baseUrl) where TTemplate : IEmailTemplateProvider, new()
        {
            var msg = new MailMessage();
            msg.To.Add(new MailAddress(destinationEmailAddress));
            await SendEmail<TTemplate>(msg, baseUrl);
        }

        private async Task SendEmail<TTemplate>(MailMessage msg, string baseUrl) where TTemplate : IEmailTemplateProvider, new()
        {
            var model = _emailModelGenerator.GenerateEmailModelFromTemplate<TTemplate>(baseUrl);
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
    }
}
