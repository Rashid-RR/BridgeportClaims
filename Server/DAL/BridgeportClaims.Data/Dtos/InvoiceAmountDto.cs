namespace BridgeportClaims.Data.Dtos
{
    public class InvoiceAmountDto
    {
        public int PrescriptionId { get; set; }
        public string RxNumber { get; set; }
        public string LabelName { get; set; }
        public string RxDate { get; set; }
        public decimal BilledAmount { get; set; }
        public string Carrier { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public bool IsReversed { get; set; }
    }
}