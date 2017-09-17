using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BridgeportClaims.Entities.DomainModels
{
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public class Invoice
    {
        public Invoice()
        {
            AcctPayable = new List<AcctPayable>();
            Prescription = new List<Prescription>();
        }
        public virtual int InvoiceId { get; set; }
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
        public virtual IList<AcctPayable> AcctPayable { get; set; }
        public virtual IList<Prescription> Prescription { get; set; }
    }
}
