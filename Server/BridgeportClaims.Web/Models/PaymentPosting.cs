using System;

namespace BridgeportClaims.Web.Models
{
    public class PaymentPosting
    {
        public string PatientName { get; set; }
        public DateTime RxDate { get; set; }
        public decimal AmountPosted { get; set; }
        public int PrescriptionId { get; set; }
    }
}