using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels.Views
{
    public class VwDocumentIndex
    {
        [Required]
        public virtual int DocumentId { get; set; }
        [Required]
        [StringLength(1000)]
        public virtual string FileName { get; set; }
        [Required]
        [StringLength(50)]
        public virtual string Extension { get; set; }
        [Required]
        [StringLength(50)]
        public virtual string FileSize { get; set; }
        [Required]
        public virtual DateTime CreationTimeLocal { get; set; }
        [Required]
        public virtual DateTime LastAccessTimeLocal { get; set; }
        [Required]
        public virtual DateTime LastWriteTimeLocal { get; set; }
        [StringLength(255)]
        public virtual string DirectoryName { get; set; }
        [Required]
        [StringLength(4000)]
        public virtual string FullFilePath { get; set; }
        [StringLength(4000)]
        public virtual string FileUrl { get; set; }
        public virtual bool? IsIndexed { get; set; }
        public virtual int? ClaimId { get; set; }
        public virtual byte? DocumentTypeId { get; set; }
        public virtual DateTime? RxDate { get; set; }
        [StringLength(100)]
        public virtual string RxNumber { get; set; }
        [StringLength(100)]
        public virtual string InvoiceNumber { get; set; }
        public virtual DateTime? InjuryDate { get; set; }
        [StringLength(255)]
        public virtual string AttorneyName { get; set; }
    }
}
