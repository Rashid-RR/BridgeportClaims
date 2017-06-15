using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class Invoice
    {
        public virtual int Id { get; set; }
        public virtual Payor Payor { get; set; }
        public virtual Claim Claim { get; set; }
        [StringLength(255)]
        public virtual string ARItemKey { get; set; }
        [Required]
        [StringLength(100)]
        public virtual string InvoiceNumber { get; set; }
        [Required]
        public virtual DateTime InvoiceDate { get; set; }
        [Required]
        public virtual decimal Amount { get; set; }
        [Required]
        public virtual DateTime CreatedOn { get; set; }
        [Required]
        public virtual DateTime UpdatedOn { get; set; }
    }
}
