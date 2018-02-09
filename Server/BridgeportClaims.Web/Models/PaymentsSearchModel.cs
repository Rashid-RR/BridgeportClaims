namespace BridgeportClaims.Web.Models
{
    public sealed class PaymentsSearchModel
    {
        public string ClaimNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RxDate { get; set; }
        public string InvoiceNumber { get; set; }
    }
}