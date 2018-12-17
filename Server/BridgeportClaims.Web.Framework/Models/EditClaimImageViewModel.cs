using System;

namespace BridgeportClaims.Web.Framework.Models
{
    public sealed class EditClaimImageViewModel
    {
        public int DocumentId { get; set; }
        public int ClaimId { get; set; }
        public byte DocumentTypeId { get; set; }
        public string RxDate { get; set; }
        public string RxNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InjuryDate { get; set; }
        public string AttorneyDate { get; set; }
    }
}