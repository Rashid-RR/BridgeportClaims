using BridgeportClaims.Web.Email.EmailModelGeneration;

namespace BridgeportClaims.Web.Email.Models
{
    public sealed class EmailViewModel
    {
        public string FullName { get; set; }
        public string Uri { get; set; }
        public EmailModelEnum EmailModelEnum { get; set; }
    }
}