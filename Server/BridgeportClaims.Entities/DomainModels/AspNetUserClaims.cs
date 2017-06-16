namespace BridgeportClaims.Entities.DomainModels
{
    public class AspNetUserClaims
    {
        public virtual int Id { get; set; }
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual string ClaimType { get; set; }
        public virtual string ClaimValue { get; set; }
    }
}