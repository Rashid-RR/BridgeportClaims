using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class OutstandingDtoTotalsResult
    {
        [Required]
        public int TotalRows { get; set; }
        [Required]
        public decimal TotalOutstanding { get; set; }
    }
}
