using System;
using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public sealed class DiariesDto
    {
        public int TotalRowCount { get; set; }
        public IList<DiaryResultDto> DiaryResults { get; set; }
    }
}