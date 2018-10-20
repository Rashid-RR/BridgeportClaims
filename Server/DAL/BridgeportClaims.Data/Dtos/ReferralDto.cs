using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class ReferralDto
    {
        [Required]
        public int ReferralId { get; set; }
        [Required]
        public string ClaimNumber { get; set; }
        [Required]
        public int JurisdictionStateId { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public DateTime InjuryDate { get; set; }
        public string Notes { get; set; }
        public string ReferredBy { get; set; }
        [Required]
        public DateTime ReferralDate { get; set; }
        [Required]
        public byte ReferralTypeId { get; set; }
        public DateTime? EligibilityStart { get; set; }
        public DateTime? EligibilityEnd { get; set; }
        [Required]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public int StateId { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string PatientPhone { get; set; }
        public string AdjustorName { get; set; }
        public string AdjustorPhone { get; set; }
        [Required]
        public DateTime CreatedOnUtc { get; set; }
        [Required]
        public DateTime UpdatedOnUtc { get; set; }
    }
}