using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class AttorneyDto
    {
        public int TotalRows { get; set; }
        public IEnumerable<AttorneyResultDto> Results { get; set; }
    }
}