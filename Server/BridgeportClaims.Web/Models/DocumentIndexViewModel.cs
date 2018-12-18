namespace BridgeportClaims.Web.Models
{
    public sealed class DocumentIndexViewModel
    {
        public int DocumentId { get; set; }
        public int ClaimId { get; set; }
        public byte DocumentTypeId { get; set; }
        public string RxDate { get; set; }
        public string RxNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string InjuryDate { get; set; }
        public string AttorneyName { get; set; }
    }
}