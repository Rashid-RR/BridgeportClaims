using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BridgeportClaims.Entities.DomainModels
{
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public class Claim
    {
        public Claim()
        {
            ClaimNote = new List<ClaimNote>();
            ClaimPayment = new List<ClaimPayment>();
            ClaimsUserHistory = new List<ClaimsUserHistory>();
            DocumentIndex = new List<DocumentIndex>();
            Episode = new List<Episode>();
            Prescription = new List<Prescription>();
        }
        [Required]
        public virtual int ClaimId { get; set; }
        [Required]
        public virtual Payor Payor { get; set; }
        public virtual Adjustor Adjustor { get; set; }
        [Required]
        public virtual UsState UsState { get; set; }
        [Required]
        public virtual Patient Patient { get; set; }
        public virtual ClaimFlex2 ClaimFlex2 { get; set; }
        [StringLength(255)]
        public virtual string PolicyNumber { get; set; }
        public virtual DateTime? DateOfInjury { get; set; }
        [Required]
        public virtual bool IsFirstParty { get; set; }
        [Required]
        [StringLength(255)]
        public virtual string ClaimNumber { get; set; }
        [StringLength(255)]
        public virtual string PreviousClaimNumber { get; set; }
        [StringLength(2)]
        public virtual string PersonCode { get; set; }
        public virtual byte? RelationCode { get; set; }
        public virtual DateTime? TermDate { get; set; }
        [Required]
        [StringLength(258)]
        public virtual string UniqueClaimNumber { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
        public virtual IList<ClaimNote> ClaimNote { get; set; }
        public virtual IList<ClaimPayment> ClaimPayment { get; set; }
        public virtual IList<ClaimsUserHistory> ClaimsUserHistory { get; set; }
        public virtual IList<DocumentIndex> DocumentIndex { get; set; }
        public virtual IList<Episode> Episode { get; set; }
        public virtual IList<Prescription> Prescription { get; set; }
    }
}
