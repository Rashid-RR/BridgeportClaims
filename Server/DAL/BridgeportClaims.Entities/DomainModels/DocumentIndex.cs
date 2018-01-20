using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class DocumentIndex
    {
        [Required]
        public virtual int DocumentId { get; set; }
        [Required]
        public virtual Document Document { get; set; }
        [Required]
        public virtual Claim Claim { get; set; }
        [Required]
        public virtual DocumentType DocumentType { get; set; }
        [Required]
        public virtual AspNetUsers IndexedByUserId { get; set; }
        public virtual DateTime? RxDate { get; set; }
        [StringLength(100)]
        public virtual string RxNumber { get; set; }
        [StringLength(100)]
        public virtual string InvoiceNumber { get; set; }
        public virtual DateTime? InjuryDate { get; set; }
        [StringLength(255)]
        public virtual string AttorneyName { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
    }
}