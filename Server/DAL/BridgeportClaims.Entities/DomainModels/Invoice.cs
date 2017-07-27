using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BridgeportClaims.Entities.DomainModels
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public class Invoice
    {
        public Invoice()
        {
            Prescription = new List<Prescription>();
        }
        [Required]
        public virtual int InvoiceId { get; set; }
        public virtual Payor Payor { get; set; }
        public virtual Claim Claim { get; set; }
        [Required]
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
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
        public virtual IList<Prescription> Prescription { get; set; }
    }
}
