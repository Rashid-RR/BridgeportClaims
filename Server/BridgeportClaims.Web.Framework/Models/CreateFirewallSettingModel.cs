namespace BridgeportClaims.Web.Framework.Models
{
    public sealed class CreateFirewallSettingModel
    {
        public string RuleName { get; set; }
        public string StartIpAddress { get; set; }
        public string EndIpAddress { get; set; }
    }
}