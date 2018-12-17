using BridgeportClaims.Web.Framework.Email.EmailModelGeneration;

namespace BridgeportClaims.Web.Framework.Models
{
    public sealed class EmailViewModel
    {
        public string FullName { get; set; }
        public string Uri { get; set; }
        public EmailModelEnum EmailModelEnum { get; set; }
    }
}