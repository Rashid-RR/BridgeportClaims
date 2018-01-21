using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class ClaimsUserHistory
    {
        [Required]
        public virtual int ClaimsUserHistoryId { get; set; }
        [Required]
        public virtual Claim Claim { get; set; }
        [Required]
        public virtual AspNetUsers UserId { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
    }
}