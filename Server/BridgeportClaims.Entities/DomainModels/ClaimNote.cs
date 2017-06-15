using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{

    public class ClaimNote
    {
        public virtual int ClaimNoteId { get; set; }
        public virtual ClaimNoteType ClaimNoteType { get; set; }
        public virtual AspNetUsers AspNetUsers { get; set; }
        [StringLength(8000)]
        public virtual string NoteText { get; set; }
        [Required]
        public virtual DateTime CreatedOn { get; set; }
        [Required]
        public virtual DateTime UpdatedOn { get; set; }
        [Required]
        public virtual DateTime DataVersion { get; set; }
    }
}
