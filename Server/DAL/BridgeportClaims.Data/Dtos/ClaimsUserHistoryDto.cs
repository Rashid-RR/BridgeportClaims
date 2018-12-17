using System;

namespace BridgeportClaims.Data.Dtos
{
    public class ClaimsUserHistoryDto
    {
        public int ClaimId { get; set; }
        public string ClaimNumber { get; set; }
        public string Name { get; set; }
        public string InjuryDate { get; set; }
        public string Carrier { get; set; }
        public DateTime CreatedOnUtc { get; set; }
    }
}