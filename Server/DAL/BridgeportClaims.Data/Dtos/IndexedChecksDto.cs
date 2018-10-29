using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class IndexedChecksDto
    {
        public int TotalRowCount { get; set; }
        public IList<IndexedChecksResultsDto> Results { get; set; }
    }
}