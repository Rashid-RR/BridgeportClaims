using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class PayorListDto
    {
        public int TotalRowCount { get; set; }
        public IEnumerable<PayorResultDto> Results { get; set; }
    }
}