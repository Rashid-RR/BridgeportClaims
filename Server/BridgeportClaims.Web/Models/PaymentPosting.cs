using System;

namespace BridgeportClaims.Web.Models
{
    public class PaymentPosting
    {
        public string PatientName { get; set; }
        public DateTime RxDate { get; set; }
        public decimal AmountPosted { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal Outstanding => InvoiceAmount - AmountPosted;
        public int PrescriptionId { get; set; }
    }
}