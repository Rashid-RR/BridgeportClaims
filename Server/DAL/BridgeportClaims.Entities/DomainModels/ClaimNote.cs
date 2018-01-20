using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class ClaimNote
    {
        [Required]
        public virtual int ClaimId { get; set; }
        [Required]
        public virtual Claim Claim { get; set; }
        public virtual ClaimNoteType ClaimNoteType { get; set; }
        [Required]
        public virtual AspNetUsers EnteredByUserId { get; set; }
        [Required]
        public virtual string NoteText { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
    }
}
