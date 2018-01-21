using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BridgeportClaims.Entities.DomainModels
{
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public class PrescriptionNote
    {
        public PrescriptionNote()
        {
            Diary = new List<Diary>();
            PrescriptionNoteMapping = new List<PrescriptionNoteMapping>();
        }
        [Required]
        public virtual int PrescriptionNoteId { get; set; }
        [Required]
        public virtual PrescriptionNoteType PrescriptionNoteType { get; set; }
        [Required]
        public virtual AspNetUsers EnteredByUserId { get; set; }
        [Required]
        [StringLength(8000)]
        public virtual string NoteText { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
        public virtual IList<Diary> Diary { get; set; }
        public virtual IList<PrescriptionNoteMapping> PrescriptionNoteMapping { get; set; }
    }
}
