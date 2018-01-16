using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BridgeportClaims.Entities.DomainModels
{
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public class Document
    {
        public Document()
        {
            DocumentIndex = new List<DocumentIndex>();
        }
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
        [Required]
        [StringLength(255)]
        public virtual string DirectoryName { get; set; }
        [Required]
        [StringLength(4000)]
        public virtual string FullFilePath { get; set; }
        [Required]
        [StringLength(500)]
        public virtual string FileUrl { get; set; }
        public virtual DateTime? DocumentDate { get; set; }
        [Required]
        public virtual long ByteCount { get; set; }
        [Required]
        public virtual bool Archived { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
        public virtual IList<DocumentIndex> DocumentIndex { get; set; }
    }
}