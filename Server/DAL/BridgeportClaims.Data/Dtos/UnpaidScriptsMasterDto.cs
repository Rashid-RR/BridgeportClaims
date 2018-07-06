using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class UnpaidScriptsMasterDto
    {
        public UnpaidScriptsDto UnpaidScripts { get; set; }
        public IList<PayorDto> Payors { get; set; }
    }
}