using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class FirewallSetting
    {
        [Required]
        [StringLength(128)]
        public string RuleName { get; set; }
        [Required]
        [StringLength(45)]
        public string StartIpAddress { get; set; }
        [Required]
        [StringLength(45)]
        public string EndIpAddress { get; set; }
    }
}