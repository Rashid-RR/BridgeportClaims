using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class SkippedPaymentDto
    {
        public int TotalRowCount { get; set; }
        public IList<SkippedPaymentResultsDto> Results { get; set; }
    }
}