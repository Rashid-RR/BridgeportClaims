using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class EpisodeTypeUsersMapping
    {
        [Required]
        public virtual byte EpisodeTypeId { get; set; }
        [Required]
        public virtual string UserId { get; set; }
        [Required]
        public virtual EpisodeType EpisodeType { get; set; }
        [Required]
        public virtual AspNetUsers User { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }

        #region NHibernate Composite Key Requirements
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var t = obj as EpisodeTypeUsersMapping;
            if (t == null) return false;
            if (EpisodeTypeId == t.EpisodeTypeId
                && UserId == t.UserId)
                return true;

            return false;
        }
        public override int GetHashCode()
        {
            int hash = GetType().GetHashCode();
            hash = (hash * 397) ^ EpisodeTypeId.GetHashCode();
            hash = (hash * 397) ^ UserId.GetHashCode();

            return hash;
        }
        #endregion
    }
}