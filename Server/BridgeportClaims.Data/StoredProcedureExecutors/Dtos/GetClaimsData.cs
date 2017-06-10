using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.StoredProcedureExecutors.Dtos
{
    public class GetClaimsData
    {
        [Required]
        public virtual int ClaimId { get; set; }
        [StringLength(311)]
        public virtual string Name { get; set; }
        [Required]
        [StringLength(255)]
        public virtual string ClaimNumber { get; set; }
        [StringLength(30)]
        public virtual string DateOfBirth { get; set; }
        [StringLength(30)]
        public virtual string InjuryDate { get; set; }
        [StringLength(55)]
        public virtual string Gender { get; set; }
        [StringLength(255)]
        public virtual string Carrier { get; set; }
        [StringLength(255)]
        public virtual string Adjustor { get; set; }
        [StringLength(30)]
        public virtual string AdjustorPhoneNumber { get; set; }
        [StringLength(30)]
        public virtual string DateEntered { get; set; }
        [StringLength(30)]
        public virtual string AdjustorFaxNumber { get; set; }
    }
}