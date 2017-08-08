using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class Payment
    {
        [Required]
        public virtual int PaymentId { get; set; }
        public virtual Prescription Prescription { get; set; }
        public virtual Claim Claim { get; set; }
        [Required]
        [StringLength(50)]
        public virtual string CheckNumber { get; set; }
        [Required]
        public virtual decimal AmountPaid { get; set; }
        public virtual DateTime? DateScanned { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
    }
}
