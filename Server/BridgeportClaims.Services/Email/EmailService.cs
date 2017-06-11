using System;
using System.Net;
using System.Net.Mail;
using BridgeportClaims.Services.Config;
using NLog;

namespace BridgeportClaims.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfigService _configService;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public EmailService(IConfigService configService)
        {
            _configService = configService;
        }

        public void SendEmail(string destinationEmailAddress, EmailTemplate template)
        {
            var sourceEmailAddress = _configService.GetConfigItem("SourceEmailAddress");
            var client = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = Convert.ToInt32(_configService.GetConfigItem("EmailPort"))
            };

            // setup Smtp authentication
            var credentials = new NetworkCredential(sourceEmailAddress, _configService.GetConfigItem("EmailPassword"));
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;

            var msg = new MailMessage
            {
                From = new MailAddress(sourceEmailAddress)
            };
            msg.To.Add(new MailAddress(destinationEmailAddress));

            msg.Subject = "This is a test Email subject";
            msg.IsBodyHtml = true;
            msg.Body = "<html><head></head><body><b>Test HTML Email</b></body>";

            try
            {
                client.Send(msg);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "SmtpClient Send() method");
                throw;
            }
        }
    }
}
