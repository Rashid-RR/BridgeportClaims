using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Web.Models
{
    public sealed class DocumentModel
    {
        [Required]
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
        public string CreationTimeLocal { get; set; }
        [Required]
        public string LastAccessTimeLocal { get; set; }
        [Required]
        public string LastWriteTimeLocal { get; set; }
        [Required]
        [StringLength(4000)]
        public string FullFilePath { get; set; }
        [Required]
        [StringLength(4000)]
        public string FileUrl { get; set; }
    }
}