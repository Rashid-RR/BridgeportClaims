using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels.Views
{
    public class VwPrescriptionNote
    {
        [Required]
        public virtual int PrescriptionId { get; set; }
        [Required]
        public virtual int PrescriptionNoteId { get; set; }
        [Required]
        [StringLength(100)]
        public virtual string RxNumber { get; set; }
        [Required]
        public virtual DateTime DateFilled { get; set; }
        [StringLength(25)]
        public virtual string LabelName { get; set; }
        [Required]
        [StringLength(255)]
        public virtual string PrescriptionNoteType { get; set; }
        [Required]
        [StringLength(8000)]
        public virtual string NoteText { get; set; }
        [Required]
        [StringLength(201)]
        public virtual string NoteAuthor { get; set; }
        [Required]
        public virtual DateTime NoteCreatedOn { get; set; }
        [Required]
        public virtual DateTime NoteUpdatedOn { get; set; }
        #region NHibernate Composite Key Requirements
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var t = obj as VwPrescriptionNote;
            if (t == null) return false;
            if (PrescriptionId == t.PrescriptionId
                && PrescriptionNoteId == t.PrescriptionNoteId)
                return true;

            return false;
        }
        public override int GetHashCode()
        {
            int hash = GetType().GetHashCode();
            hash = (hash * 397) ^ PrescriptionId.GetHashCode();
            hash = (hash * 397) ^ PrescriptionNoteId.GetHashCode();

            return hash;
        }
        #endregion
    }
}