namespace BridgeportClaims.Web.Framework.Models
{
    public class PrescriptionPaymentViewModel
    {
        public int PrescriptionPaymentId { get; set; }
        public string CheckNumber { get; set; }
        public decimal AmountPaid { get; set; }
        public string DatePosted { get; set; }
        public int PrescriptionId { get; set; }
    }
}