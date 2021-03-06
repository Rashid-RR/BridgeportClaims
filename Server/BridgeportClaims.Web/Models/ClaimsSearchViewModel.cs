﻿namespace BridgeportClaims.Web.Models
{
    public sealed class ClaimsSearchViewModel
    {
        public int? ClaimId { get; set; }
        public string ClaimNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RxNumber { get; set; }
        public string InvoiceNumber { get; set; }
    }
}