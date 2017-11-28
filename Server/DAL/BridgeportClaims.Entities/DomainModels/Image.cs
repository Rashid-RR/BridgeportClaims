using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class Image
    {
        public virtual int ImageId { get; set; }
        public virtual Claim Claim { get; set; }
        public virtual ImageType ImageType { get; set; }
        [Required]
        public virtual DateTime CreatedDateLocal { get; set; }
        public virtual bool? IsIndexed { get; set; }
        public virtual DateTime? RxDate { get; set; }
        [StringLength(100)]
        public virtual string RxNumber { get; set; }
        [StringLength(100)]
        public virtual string InvoiceNumber { get; set; }
        public virtual DateTime? InjuryDate { get; set; }
        [StringLength(255)]
        public virtual string AttorneyName { get; set; }
        [Required]
        [StringLength(1000)]
        public virtual string FileName { get; set; }
        [StringLength(4000)]
        public virtual string FileUrl { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
    }
}