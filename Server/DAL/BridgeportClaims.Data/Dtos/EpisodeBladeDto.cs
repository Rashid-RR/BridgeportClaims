using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class EpisodeBladeDto
    {
        [Required]
        public int Id { get; set; }
        public DateTime? Created { get; set; }
        [StringLength(201)]
        public string Owner { get; set; }
        [StringLength(255)]
        public string Type { get; set; }
        [StringLength(25)]
        public string Role { get; set; }
        [StringLength(60)]
        public string Pharmacy { get; set; }
        [StringLength(100)]
        public string RxNumber { get; set; }
        public bool? Resolved { get; set; }
        public int? NoteCount { get; set; }
    }
}
