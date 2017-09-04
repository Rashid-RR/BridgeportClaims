using System;
using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public sealed class ClaimsSearchResultsDto
    {
        public string CheckNumber { get; set; }
        public IList<PrescriptionPostingAmountsDto> PrescriptionPostingAmounts { get; set; }
    }
}