using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BridgeportClaims.Entities.DomainModels
{
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public class ClaimImageType
    {
        
        public ClaimImageType() { ClaimImage = new List<ClaimImage>(); }
        [Required]
        public virtual int ClaimImageTypeId { get; set; }
        [Required]
        [StringLength(255)]
        public virtual string TypeName { get; set; }
        [Required]
        [StringLength(10)]
        public virtual string Code { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
        public virtual IList<ClaimImage> ClaimImage { get; set; }
    }
}