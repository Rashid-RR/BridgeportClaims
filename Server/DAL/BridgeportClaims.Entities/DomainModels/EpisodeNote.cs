using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class EpisodeNote
    {
        [Required]
        public virtual int EpisodeNoteId { get; set; }
        [Required]
        public virtual Episode Episode { get; set; }
        [Required]
        public virtual AspNetUsers WrittenByUser { get; set; }
        [Required]
        [StringLength(8000)]
        public virtual string NoteText { get; set; }
        [Required]
        public virtual DateTime Created { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
    }
}