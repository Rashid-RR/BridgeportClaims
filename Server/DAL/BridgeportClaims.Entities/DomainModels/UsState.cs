using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BridgeportClaims.Entities.DomainModels
{
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public class UsState
    {
        public UsState()
        {
            Claim = new List<Claim>();
            Patient = new List<Patient>();
            Payor = new List<Payor>();
            Pharmacy = new List<Pharmacy>();
        }
        [Required]
        public virtual int StateId { get; set; }
        [Required]
        [StringLength(2)]
        public virtual string StateCode { get; set; }
        [Required]
        [StringLength(64)]
        public virtual string StateName { get; set; }
        [Required]
        public virtual bool IsTerritory { get; set; }
        public virtual IList<Claim> Claim { get; set; }
        public virtual IList<Patient> Patient { get; set; }
        public virtual IList<Payor> Payor { get; set; }
        public virtual IList<Pharmacy> Pharmacy { get; set; }
    }
}
