using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class ClaimImage
    {
        public virtual int ClaimImageId { get; set; }
        public virtual Claim Claim { get; set; }
        public virtual int? ImageNumber { get; set; }
        [StringLength(255)]
        public virtual string ImageType { get; set; }
        public virtual DateTime? Daterec { get; set; }
        [Required]
        public virtual DateTime CreatedOn { get; set; }
        [Required]
        public virtual DateTime UpdatedOn { get; set; }
        [Required]
        public virtual DateTime DataVersion { get; set; }
    }
}