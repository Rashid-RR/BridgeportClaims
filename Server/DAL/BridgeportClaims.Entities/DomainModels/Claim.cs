﻿using System;
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
            ClaimImage = new List<ClaimImage>();
            ClaimNote = new List<ClaimNote>();
            Episode = new List<Episode>();
            Invoice = new List<Invoice>();
            Payment = new List<Payment>();
            Prescription = new List<Prescription>();
        }
        [Required]
        public virtual int ClaimId { get; set; }
        [Required]
        public virtual Payor Payor { get; set; }
        public virtual Adjustor Adjustor { get; set; }
        public virtual UsState UsState { get; set; }
        [Required]
        public virtual Patient Patient { get; set; }
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
        public virtual int? PersonCode { get; set; }
        public virtual byte? RelationCode { get; set; }
        public virtual DateTime? TermDate { get; set; }
        [Required]
        [StringLength(258)]
        public virtual string UniqueClaimNumber { get; set; }
        [Required]
        public virtual DateTime CreatedOn { get; set; }
        [Required]
        public virtual DateTime UpdatedOn { get; set; }
        public virtual IList<ClaimImage> ClaimImage { get; set; }
        public virtual IList<ClaimNote> ClaimNote { get; set; }
        public virtual IList<Episode> Episode { get; set; }
        public virtual IList<Invoice> Invoice { get; set; }
        public virtual IList<Payment> Payment { get; set; }
        public virtual IList<Prescription> Prescription { get; set; }
    }
}
