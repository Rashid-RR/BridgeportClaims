using System.Collections.Generic;

namespace BridgeportClaims.Web.Models
{
    public sealed class DrLetterModel
    {
        public int ClaimId { get; set; }
        public int FirstPrescriptionId { get; set; }
        public IList<int> PrescriptionIds { get; set; }
    }
}