using System;
using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public sealed class UnpaidScriptsDto
    {
        public int TotalRowCount { get; set; }
        public IList<UnpaidScriptResultDto> UnpaidScriptResults { get; set; }
    }
}