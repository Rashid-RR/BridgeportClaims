namespace BridgeportClaims.Entities.DomainModels
{
    public class AspNetUserRoles
    {
        public virtual string UserId { get; set; }
        public virtual string RoleId { get; set; }
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual AspNetRoles AspNetRoles { get; set; }

        #region NHibernate Composite Key Requirements
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var t = obj as AspNetUserRoles;
            if (t == null) return false;
            if (UserId == t.UserId
                && RoleId == t.RoleId)
                return true;

            return false;
        }
        public override int GetHashCode()
        {
            int hash = GetType().GetHashCode();
            hash = (hash * 397) ^ UserId.GetHashCode();
            hash = (hash * 397) ^ RoleId.GetHashCode();

            return hash;
        }
        #endregion
    }
}
