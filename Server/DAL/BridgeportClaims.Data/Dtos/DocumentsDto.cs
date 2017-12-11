using System;
using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public sealed class DocumentsDto
    {
        public IList<DocumentTypeDto> DocumentTypes { get; set; }
        public int ClaimId { get; set; }
        public int TotalRowCount { get; set; }
        public IList<DocumentResultDto> DocumentResults { get; set; }
    }
}
