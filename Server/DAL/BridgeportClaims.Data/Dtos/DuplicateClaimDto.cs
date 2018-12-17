using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class DuplicateClaimDto
    {
        public int TotalRowCount { get; set; }
        public IList<DuplicateClaimResultsDto> Results { get; set; }
    }
}