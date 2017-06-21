using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class ClaimImageType
    {
        [Required]
        public virtual int ClaimImageTypeId { get; set; }
        [Required]
        [StringLength(255)]
        public virtual string TypeName { get; set; }
        [Required]
        [StringLength(10)]
        public virtual string Code { get; set; }
        [Required]
        public virtual DateTime CreatedOn { get; set; }
        [Required]
        public virtual DateTime UpdatedOn { get; set; }
    }
}