using System;

namespace BridgeportClaims.Web.Models
{
    [Serializable]
    public sealed class DocumentIndexViewModel
    {
        public int DocumentId { get; set; }
        public int ClaimId { get; set; }
        public byte DocumentTypeId { get; set; }
        public DateTime? RxDate { get; set; }
        public string RxNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? InjuryDate { get; set; }
        public string AttorneyName { get; set; }
    }
}