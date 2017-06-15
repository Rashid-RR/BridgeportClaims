using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class PrescriptionNote
    {
        public virtual int Id { get; set; }
        public virtual PrescriptionNoteType PrescriptionNoteType { get; set; }
        public virtual AspNetUsers AspNetUsers { get; set; }
        [Required]
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
