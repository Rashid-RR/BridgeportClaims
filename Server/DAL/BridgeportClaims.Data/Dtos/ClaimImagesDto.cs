using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class ClaimImagesDto
    {
        public int TotalRowCount { get; set; }
        public IList<ClaimImageResultDto> ClaimImages { get; set; }
    }
}