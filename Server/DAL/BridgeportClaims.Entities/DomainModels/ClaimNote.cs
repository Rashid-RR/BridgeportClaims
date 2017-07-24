using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class ClaimNote
    {
        [Required]
        public virtual int ClaimId { get; set; }
        public virtual Claim Claim { get; set; }
        public virtual ClaimNoteType ClaimNoteType { get; set; }
        public virtual AspNetUsers AspNetUsers { get; set; }
        [Required]
        public virtual string NoteText { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
    }
}
