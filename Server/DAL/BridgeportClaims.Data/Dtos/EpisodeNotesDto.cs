using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public sealed class EpisodeNotesDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(205)]
        public string Owner { get; set; }
        public DateTime? EpisodeCreated { get; set; }
        [Required]
        [StringLength(315)]
        public string PatientName { get; set; }
        [Required]
        [StringLength(255)]
        public string ClaimNumber { get; set; }
        [Required]
        [StringLength(205)]
        public string WrittenBy { get; set; }
        [Required]
        public DateTime NoteCreated { get; set; }
        [Required]
        [StringLength(8000)]
        public string NoteText { get; set; }
    }
}
