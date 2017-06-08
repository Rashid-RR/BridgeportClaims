using System;
using System.ComponentModel.DataAnnotations;
using BridgeportClaims.Entities.Domain;

namespace BridgeportClaims.Entities.DomainModels
{
    public class Payment : IEntity
    {
        public virtual int Id { get; set; }
        public virtual Claim Claim { get; set; }
        [Required]
        [StringLength(100)]
        public virtual string CheckNumber { get; set; }
        [Required]
        public virtual DateTime CheckDate { get; set; }
        [Required]
        public virtual decimal AmountPaid { get; set; }
        [Required]
        [StringLength(100)]
        public virtual string InvoiceNumber { get; set; }
        [Required]
        public virtual DateTime CreatedOn { get; set; }
        [Required]
        public virtual DateTime UpdatedOn { get; set; }
    }
}
