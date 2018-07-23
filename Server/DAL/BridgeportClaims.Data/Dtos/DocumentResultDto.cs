using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class DocumentResultDto
    {
        private string _fileUrl;
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
        public DateTime CreationTimeLocal { get; set; }
        [Required]
        public DateTime LastAccessTimeLocal { get; set; }
        [Required]
        public DateTime LastWriteTimeLocal { get; set; }
        [Required]
        [StringLength(4000)]
        public string FullFilePath { get; set; }
        [Required]
        [StringLength(4000)]
        public string FileUrl
        {
            get => HttpUtility.UrlEncode(_fileUrl);
            set => _fileUrl = value;
        }
    }
}
