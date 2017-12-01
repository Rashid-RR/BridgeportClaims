using System;
using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public sealed class DocumentsDto
    {
        public int TotalRowCount { get; set; }
        public IList<DocumentResultDto> DocumentResults { get; set; }
    }
}
