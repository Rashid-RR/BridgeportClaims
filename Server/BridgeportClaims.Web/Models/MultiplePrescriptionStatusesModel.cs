using System.Collections.Generic;

namespace BridgeportClaims.Web.Models
{
    public class MultiplePrescriptionStatusesModel
    {
        public IList<int> PrescriptionIds { get; set; }
        public int PrescriptionStatusId { get; set; }
    }
}