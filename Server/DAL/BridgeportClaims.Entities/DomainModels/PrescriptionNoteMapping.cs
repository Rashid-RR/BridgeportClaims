using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class PrescriptionNoteMapping
    {
        public virtual int PrescriptionId { get; set; }
        public virtual int PrescriptionNoteId { get; set; }
        public virtual Prescription Prescription { get; set; }
        public virtual PrescriptionNote PrescriptionNote { get; set; }
        [Required]
        public virtual DateTime CreatedOn { get; set; }
        [Required]
        public virtual DateTime UpdatedOn { get; set; }

        #region NHibernate Composite Key Requirements

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var t = obj as PrescriptionNoteMapping;
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