using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels.Views
{
    public class VwClaimInfo
    {
        [Required]
        public virtual int ClaimId { get; set; }
        [Required]
        [StringLength(255)]
        public virtual string ClaimNumber { get; set; }
        [StringLength(311)]
        public virtual string Name { get; set; }
        [Required]
        [StringLength(255)]
        public virtual string Carrier { get; set; }
        public virtual DateTime? InjuryDate { get; set; }
    }
}