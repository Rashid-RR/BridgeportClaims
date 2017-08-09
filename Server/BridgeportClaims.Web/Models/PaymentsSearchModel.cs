using System;

namespace BridgeportClaims.Web.Models
{
    [Serializable]
    public sealed class PaymentsSearchModel
    {
        public string ClaimNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? RxDate { get; set; }
        public string InvoiceNumber { get; set; }
    }
}