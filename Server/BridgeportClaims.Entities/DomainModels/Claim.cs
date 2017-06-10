using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using BridgeportClaims.Entities.Domain;

namespace BridgeportClaims.Entities.DomainModels
{
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public class Claim : IEntity
    {
        public Claim()
        {
            Payment = new List<Payment>();
        }
        public virtual int Id { get; set; }
        public virtual Payor Payor { get; set; }
        public virtual Adjustor Adjustor { get; set; }
        public virtual UsState UsState { get; set; }
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
        public virtual IList<Payment> Payment { get; set; }
    }
}
