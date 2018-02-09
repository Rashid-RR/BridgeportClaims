using System.Collections.Generic;

namespace BridgeportClaims.Web.Models
{
    public sealed class AmountRemainingModel
    {
        public IList<int> ClaimIds { get; set; }
        public string CheckNumber { get; set; }
    }
}