using System.Collections.Generic;

namespace BridgeportClaims.Web.Models
{
    public sealed class PostedPaymentViewModel
    {
        public IList<int> PrescriptionIds { get; set; }
        public decimal PostedAmount { get; set; }
        public decimal Outstanding { get; set; }
        public decimal AmountRemaining { get; set; }
    }
}