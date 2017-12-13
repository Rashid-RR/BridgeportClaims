using System;
using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public sealed class ClaimImagesDto
    {
        public int TotalRowCount { get; set; }
        public IList<ClaimImageResultDto> ClaimImages { get; set; }
    }
}