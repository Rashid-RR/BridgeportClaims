using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public sealed class ClaimImageResultDto
    {
        [Required]
        public int DocumentId { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public string Type { get; set; }
        public DateTime? RxDate { get; set; }
        public string RxNumber { get; set; }
        [Required]
        [StringLength(1000)]
        public string FileName { get; set; }
    }
}