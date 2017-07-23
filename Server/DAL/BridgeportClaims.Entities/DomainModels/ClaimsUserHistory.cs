using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class ClaimsUserHistory
    {
        [Required]
        public virtual int ClaimsUserHistoryId { get; set; }
        public virtual Claim Claim { get; set; }
        public virtual AspNetUsers AspNetUsers { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
    }
}