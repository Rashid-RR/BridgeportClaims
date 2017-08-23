using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels.Views
{
    public class VwPayment
    {
        [Required]
        public virtual int PaymentId { get; set; }
        [Required]
        [StringLength(50)]
        public virtual string CheckNumber { get; set; }
        [Required]
        public virtual decimal AmountPaid { get; set; }
        public virtual DateTime? DatePosted { get; set; }
        public virtual int? PrescriptionId { get; set; }
        public virtual int? ClaimId { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
    }
}