using System;

namespace BridgeportClaims.Web.Models
{
    public sealed class MergeClaimModel
    {
        public int ClaimId { get; set; }
        public int DuplicateClaimId { get; set; }
        public string ClaimNumber { get; set; }
        public int PatientId { get; set; }
        public string InjuryDate { get; set; }
        public int? AdjustorId { get; set; }
        public int PayorId { get; set; }
        public int? ClaimFlex2Id { get; set; }
    }
}