using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace BridgeportClaims.Web.Email.Model
{
    public class EmailModel
    {
        public string SourceEmailAddress { get; set; }
        public string DestinationEmailAddress { get; set; }
        public IList<string> DestinationEmailAddresses { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public bool IsBodyHtml { get; set; }
        public string EmailHost { get; set; }
        public int EmailPort { get; set; }
        public NetworkCredential EmailCredential { get; set; }
        public bool UseDefaultCredential { get; set; }
        public SmtpDeliveryMethod DeliveryMethod { get; set; }
        public bool EnableSsl { get; set; }
        public string BaseUrl { get; set; }
        public string SourceEmailDisplayName { get; internal set; }
    }
}