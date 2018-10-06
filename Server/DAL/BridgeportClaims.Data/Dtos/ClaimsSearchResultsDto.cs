using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class ClaimsSearchResultsDto
    {
        public string CheckNumber { get; set; }
        public IList<PrescriptionPostingAmountsDto> PrescriptionPostingAmounts { get; set; }
    }
}