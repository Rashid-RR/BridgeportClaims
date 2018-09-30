using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class DocumentsDto
    {
        public IList<DocumentTypeDto> DocumentTypes { get; set; }
        public int TotalRowCount { get; set; }
        public IList<DocumentResultDto> DocumentResults { get; set; }
    }
}
