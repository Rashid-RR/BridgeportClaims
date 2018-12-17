using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace BridgeportClaims.Web.Framework.Models
{
    public class EmailModel
    {
        internal string SourceEmailAddress { get; set; }
        internal string DestinationEmailAddress { get; set; }
        internal IList<string> DestinationEmailAddresses { get; set; }
        internal string EmailSubject { get; set; }
        internal string EmailBody { get; set; }
        internal bool IsBodyHtml { get; set; }
        internal string EmailHost { get; set; }
        internal int EmailPort { get; set; }
        internal NetworkCredential EmailCredential { get; set; }
        internal bool UseDefaultCredential { get; set; }
        internal SmtpDeliveryMethod DeliveryMethod { get; set; }
        internal bool EnableSsl { get; set; }
        internal string SourceEmailDisplayName { get; set; }
        internal object Model { get; set; }
    }
}