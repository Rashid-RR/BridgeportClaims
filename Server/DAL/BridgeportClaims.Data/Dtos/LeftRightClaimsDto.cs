using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class LeftRightClaimsDto
    {
        [Required]
        public int LeftClaimId { get; set; }
        [Required]
        [StringLength(255)]
        public string LeftClaimNumber { get; set; }
        [Required]
        public int LeftPatientId { get; set; }
        [Required]
        [StringLength(312)]
        public string LeftPatientName { get; set; }
        public DateTime? LeftDateOfBirth { get; set; }
        public DateTime? LeftInjuryDate { get; set; }
        public int? LeftAdjustorId { get; set; }
        [StringLength(255)]
        public string LeftAdjustorName { get; set; }
        [Required]
        public int LeftPayorId { get; set; }
        [Required]
        [StringLength(255)]
        public string LeftCarrier { get; set; }
        [StringLength(10)]
        public string LeftClaimFlex2Value { get; set; }
        public int? LeftClaimFlex2Id { get; set; }
        [Required]
        public int RightClaimId { get; set; }
        [Required]
        [StringLength(255)]
        public string RightClaimNumber { get; set; }
        [Required]
        public int RightPatientId { get; set; }
        [Required]
        [StringLength(312)]
        public string RightPatientName { get; set; }
        public DateTime? RightDateOfBirth { get; set; }
        public DateTime? RightInjuryDate { get; set; }
        public int? RightAdjustorId { get; set; }
        [StringLength(255)]
        public string RightAdjustorName { get; set; }
        [Required]
        public int RightPayorId { get; set; }
        [Required]
        [StringLength(255)]
        public string RightCarrier { get; set; }
        [StringLength(10)]
        public string RightClaimFlex2Value { get; set; }
        public int? RightClaimFlex2Id { get; set; }
    }
}