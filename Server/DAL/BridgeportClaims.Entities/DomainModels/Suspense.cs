using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class Suspense
    {
        [Required]
        public virtual int SuspenseId { get; set; }
        public virtual Claim Claim { get; set; }
        public virtual AspNetUsers UserId { get; set; }
        [Required]
        [StringLength(50)]
        public virtual string CheckNumber { get; set; }
        [Required]
        public virtual decimal AmountRemaining { get; set; }
        [Required]
        public virtual DateTime SuspenseDate { get; set; }
        [StringLength(255)]
        public virtual string NoteText { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
    }
}