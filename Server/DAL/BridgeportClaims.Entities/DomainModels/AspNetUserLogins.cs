using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class AspNetUserLogins
    {
        [Required]
        [StringLength(128)]
        public virtual string LoginProvider { get; set; }
        [Required]
        [StringLength(128)]
        public virtual string ProviderKey { get; set; }
        [Required]
        [StringLength(128)]
        public virtual string UserId { get; set; }
        public virtual AspNetUsers AspNetUsers { get; set; }
        #region NHibernate Composite Key Requirements
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var t = obj as AspNetUserLogins;
            if (t == null) return false;
            if (LoginProvider == t.LoginProvider
                && ProviderKey == t.ProviderKey
                && UserId == t.UserId)
                return true;

            return false;
        }
        public override int GetHashCode()
        {
            var hash = GetType().GetHashCode();
            hash = (hash * 397) ^ LoginProvider.GetHashCode();
            hash = (hash * 397) ^ ProviderKey.GetHashCode();
            hash = (hash * 397) ^ UserId.GetHashCode();

            return hash;
        }
        #endregion
    }
}