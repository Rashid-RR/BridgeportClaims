using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class PrescriptionPayment
    {
        [Required]
        public virtual int PrescriptionPaymentId { get; set; }
        [Required]
        public virtual Prescription Prescription { get; set; }
        [Required]
        [StringLength(50)]
        public virtual string CheckNumber { get; set; }
        [Required]
        public virtual decimal AmountPaid { get; set; }
        public virtual DateTime? DatePosted { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
    }
}