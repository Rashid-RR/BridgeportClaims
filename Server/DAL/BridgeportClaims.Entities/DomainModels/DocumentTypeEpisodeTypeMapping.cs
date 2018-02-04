using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class DocumentTypeEpisodeTypeMapping
    {
        [Required]
        public virtual byte DocumentTypeId { get; set; }
        [Required]
        public virtual byte EpisodeTypeId { get; set; }
        [Required]
        public virtual DocumentType DocumentType { get; set; }
        [Required]
        public virtual EpisodeType EpisodeType { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
        [Required]
        public virtual DateTime DataVersion { get; set; }
        #region NHibernate Composite Key Requirements
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var t = obj as DocumentTypeEpisodeTypeMapping;
            if (t == null) return false;
            if (DocumentTypeId == t.DocumentTypeId
                && EpisodeTypeId == t.EpisodeTypeId)
                return true;

            return false;
        }
        public override int GetHashCode()
        {
            int hash = GetType().GetHashCode();
            hash = (hash * 397) ^ DocumentTypeId.GetHashCode();
            hash = (hash * 397) ^ EpisodeTypeId.GetHashCode();

            return hash;
        }
        #endregion
    }
}