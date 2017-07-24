using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BridgeportClaims.Entities.DomainModels
{
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public class Adjustor
    {
        public Adjustor() { Claim = new List<Claim>(); }
        [Required]
        public virtual int AdjustorId { get; set; }
        public virtual Payor Payor { get; set; }
        [Required]
        [StringLength(255)]
        public virtual string AdjustorName { get; set; }
        [StringLength(30)]
        public virtual string PhoneNumber { get; set; }
        [StringLength(30)]
        public virtual string FaxNumber { get; set; }
        [StringLength(155)]
        public virtual string EmailAddress { get; set; }
        [StringLength(10)]
        public virtual string Extension { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
        public virtual IList<Claim> Claim { get; set; }
    }
}