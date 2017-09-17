using System;

namespace BridgeportClaims.Web.Models
{
    public class PrescriptionPaymentViewModel
    {
        public int PrescriptionPaymentId { get; set; }
        public string CheckNumber { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime? DatePosted { get; set; }
        public int PrescriptionId { get; set; }
    }
}