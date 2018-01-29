using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public sealed class EpisodeResultsDto
    {
        [Required]
        public int EpisodeId { get; set; }
        [Required]
        [StringLength(202)]
        public string Owner { get; set; }
        public DateTime? Created { get; set; }
        [Required]
        [StringLength(312)]
        public string PatientName { get; set; }
        [Required]
        [StringLength(255)]
        public string ClaimNumber { get; set; }
        [StringLength(255)]
        public string Type { get; set; }
        [StringLength(60)]
        public string Pharmacy { get; set; }
        [Required]
        [StringLength(255)]
        public string Carrier { get; set; }
        [Required]
        [StringLength(8000)]
        public string EpisodeNote { get; set; }
    }
}