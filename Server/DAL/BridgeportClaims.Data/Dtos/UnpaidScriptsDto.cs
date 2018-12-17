using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class UnpaidScriptsDto
    {
        public int TotalRowCount { get; set; }
        public IList<UnpaidScriptResultDto> UnpaidScriptResults { get; set; }
    }
}