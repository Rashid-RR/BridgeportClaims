using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class InvoiceIndex
    {
        [Required]
        public virtual int DocumentId { get; set; }
        [Required]
        public virtual Document Document { get; set; }
        [Required]
        public virtual AspNetUsers ModifiedByUser { get; set; }
        [Required]
        [StringLength(100)]
        public virtual string InvoiceNumber { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
    }
}