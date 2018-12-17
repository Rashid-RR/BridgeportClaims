using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class ShortPayDto
    {
        public int TotalRowCount { get; set; }
        public IList<ShortPayResultDto> Results { get; set; }
    }
}