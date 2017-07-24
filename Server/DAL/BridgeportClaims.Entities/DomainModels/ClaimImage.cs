using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class ClaimImage
    {
        [Required]
        public virtual int ClaimImageId { get; set; }
        public virtual Claim Claim { get; set; }
        public virtual ClaimImageType ClaimImageType { get; set; }
        public virtual DateTime? DateRecorded { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
    }
}