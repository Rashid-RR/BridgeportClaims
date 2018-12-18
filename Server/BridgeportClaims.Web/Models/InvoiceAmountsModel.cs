namespace BridgeportClaims.Web.Models
{
    public class InvoiceAmountsModel
    {
        public int ClaimId { get; set; }
        public string RxNumber { get; set; }
        public string RxDate { get; set; } = null;
        public string InvoiceNumber { get; set; } = null;
    }
}