using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class SkippedPaymentResultsDto
    {
        [Required]
        public string ClaimNumber { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstName { get; set; }
        public decimal? AmountPaid { get; set; }
        [Required]
        public string RxNumber { get; set; }
        public string RxDate { get; set; }
        public string AdjustorName { get; set; }
        public string AdjustorPh { get; set; }
        [Required]
        public string Carrier { get; set; }
        public string CarrierPh { get; set; }
        public DateTime? ReversedDate { get; set; }
        public string PrescriptionStatus { get; set; }
        public string InvoiceNumber { get; set; }
    }
}