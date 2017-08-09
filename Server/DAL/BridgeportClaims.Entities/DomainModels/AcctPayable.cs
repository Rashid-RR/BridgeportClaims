using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class AcctPayable
    {
        public virtual int AcctPayableId { get; set; }
        public virtual Claim Claim { get; set; }
        public virtual Invoice Invoice { get; set; }
        [Required]
        [StringLength(100)]
        public virtual string CheckNumber { get; set; }
        [Required]
        public virtual DateTime CheckDate { get; set; }
        [Required]
        public virtual decimal AmountPaid { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
    }
}
