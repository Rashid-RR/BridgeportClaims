using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Business.Dto
{
    public sealed class DocumentDto
    {
        public int DocumentId { get; set; }
        [Required]
        [StringLength(1000)]
        public string FileName { get; set; }
        [Required]
        [StringLength(50)]
        public string Extension { get; set; }
        [Required]
        [StringLength(50)]
        public string FileSize { get; set; }
        [Required]
        public DateTime CreationTimeLocal { get; set; }
        [Required]
        public DateTime LastAccessTimeLocal { get; set; }
        [Required]
        public DateTime LastWriteTimeLocal { get; set; }
        [StringLength(255)]
        public string DirectoryName { get; set; }
        [Required]
        [StringLength(4000)]
        public string FullFilePath { get; set; }
        [Required]
        [StringLength(4000)]
        public string FileUrl { get; set; }
        [Required]
        public long ByteCount { get; set; }
        [Required]
        public byte FileTypeId { get; set; }
    }
}