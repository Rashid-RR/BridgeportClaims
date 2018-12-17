using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class AdjustorDto
    {
        public int TotalRows { get; set; }
        public IEnumerable<AdjustorResultDto> Results { get; set; }   
    }
}
