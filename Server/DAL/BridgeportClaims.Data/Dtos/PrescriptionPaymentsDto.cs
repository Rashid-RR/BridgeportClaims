using System;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class PrescriptionPaymentsDto
    {
        public int PrescriptionPaymentId { get; set; }
        public int PrescriptionId { get; set; }
        public DateTime PostedDate { get; set; }
        public string CheckNumber { get; set; }
        public decimal CheckAmt { get; set; }
        public string RxNumber { get; set; }
        public DateTime RxDate { get; set; }
        public string InvoiceNumber { get; set; }
        public bool IsReversed { get; set; }
        public decimal PayableAmount { get; set; }
    }
}