using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class AspNetUserClaims
    {
        [Required]
        public virtual int Id { get; set; }
        public virtual AspNetUsers AspNetUsers { get; set; }
        [Required]
        [StringLength(4000)]
        public virtual string ClaimType { get; set; }
        [Required]
        [StringLength(4000)]
        public virtual string ClaimValue { get; set; }
    }
}