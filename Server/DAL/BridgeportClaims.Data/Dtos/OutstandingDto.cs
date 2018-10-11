using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class OutstandingDto
    {
        public decimal TotalOutstanding { get; set; }
        public string TotalOutstandingAmount => TotalOutstanding.ToString("F2");
        public int TotalRows { get; set; }
        public IEnumerable<OutstandingDtoResult> Results { get; set; }
         
    }
}
