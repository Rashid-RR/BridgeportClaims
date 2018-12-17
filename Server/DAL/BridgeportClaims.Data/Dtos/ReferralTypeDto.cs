using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class ReferralTypeDto
    {
        [Required]
        public int ReferralTypeId { get; set; }
        public string TypeName { get; set; }
    }
}
