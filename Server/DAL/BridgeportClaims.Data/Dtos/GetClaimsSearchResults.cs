using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.Dtos
{
    public class GetClaimsSearchResults
    {
        [Required]
        public virtual int ClaimId { get; set; }
        public virtual int PayorId { get; set; }
        public virtual int PrescriptionId { get; set; }
        [Required]
        [StringLength(255)]
        public virtual string ClaimNumber { get; set; }
        [StringLength(155)]
        public virtual string LastName { get; set; }
        [StringLength(155)]
        public virtual string FirstName { get; set; }
        [StringLength(255)]
        public virtual string Carrier { get; set; }
        [StringLength(15)]
        public virtual DateTime? InjuryDate { get; set; }
    }
}