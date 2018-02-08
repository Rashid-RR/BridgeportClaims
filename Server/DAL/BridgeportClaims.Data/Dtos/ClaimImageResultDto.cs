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
        [StringLength(255)]
        public string Type { get; set; }
        public DateTime? RxDate { get; set; }
        [StringLength(100)]
        public string RxNumber { get; set; }
        [StringLength(100)]
        public string InvoiceNumber { get; set; }
        public DateTime? InjuryDate { get; set; }
        [StringLength(255)]
        public string AttorneyName { get; set; }
        [Required]
        [StringLength(1000)]
        public string FileName { get; set; }
        [Required]
        [StringLength(500)]
        public string FileUrl { get; set; }
    }
}