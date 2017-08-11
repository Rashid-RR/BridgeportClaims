using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class VwImportFile
    {
        [Required]
        public virtual int ImportFileId { get; set; }
        [Required]
        [StringLength(255)]
        public virtual string FileName { get; set; }
        [StringLength(30)]
        public virtual string FileExtension { get; set; }
        [Required]
        [StringLength(100)]
        public virtual string FileSize { get; set; }
        [Required]
        [StringLength(255)]
        public virtual string FileType { get; set; }
        [Required]
        [StringLength(5)]
        public virtual string Processed { get; set; }
        public virtual DateTime? CreatedOnLocal { get; set; }
        public virtual DateTime? UpdatedOnLocal { get; set; }
    }
}
