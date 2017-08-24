using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels.Views
{
    public class VwClaimInfo
    {
        [Required]
        public virtual int RowId { get; set; }
        [Required]
        public virtual int ClaimId { get; set; }
        [Required]
        [StringLength(255)]
        public virtual string ClaimNumber { get; set; }
        [StringLength(500)]
        public virtual string Name { get; set; }
        [StringLength(255)]
        public virtual string Carrier { get; set; }
        [StringLength(30)]
        public virtual string InjuryDate { get; set; }
    }
}