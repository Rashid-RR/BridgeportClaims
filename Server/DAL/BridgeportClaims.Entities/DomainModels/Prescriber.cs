using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class Prescriber
    {
        [Required]
        public virtual string PrescriberNpi { get; set; }
        public virtual UsState StateId { get; set; }
        public virtual DateTime? ExpDate { get; set; }
        [StringLength(255)]
        public virtual string PrescriberName { get; set; }
        [StringLength(255)]
        public virtual string Addr1 { get; set; }
        [StringLength(255)]
        public virtual string Addr2 { get; set; }
        [StringLength(255)]
        public virtual string City { get; set; }
        [StringLength(255)]
        public virtual string Zip { get; set; }
        [StringLength(255)]
        public virtual string Fldohnum { get; set; }
        [StringLength(255)]
        public virtual string Phone { get; set; }
        [StringLength(255)]
        public virtual string Fax { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
    }
}