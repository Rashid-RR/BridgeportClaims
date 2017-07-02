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
            PrescriptionNoteMapping = new List<PrescriptionNoteMapping>();
        }
        public virtual int PrescriptionNoteId { get; set; }
        public virtual PrescriptionNoteType PrescriptionNoteType { get; set; }
        public virtual AspNetUsers AspNetUsers { get; set; }
        [Required]
        [StringLength(8000)]
        public virtual string NoteText { get; set; }
        [Required]
        public virtual DateTime CreatedOn { get; set; }
        [Required]
        public virtual DateTime UpdatedOn { get; set; }
        public virtual IList<PrescriptionNoteMapping> PrescriptionNoteMapping { get; set; }
    }
}
