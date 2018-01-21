using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class ImportFile
    {
        [Required]
        public virtual int ImportFileId { get; set; }
        [Required]
        public virtual ImportFileType ImportFileType { get; set; }
        [Required]
        public virtual byte[] FileBytes { get; set; }
        [Required]
        [StringLength(255)]
        public virtual string FileName { get; set; }
        [StringLength(30)]
        public virtual string FileExtension { get; set; }
        [Required]
        [StringLength(100)]
        public virtual string FileSize { get; set; }
        [Required]
        public virtual bool Processed { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
    }
}
