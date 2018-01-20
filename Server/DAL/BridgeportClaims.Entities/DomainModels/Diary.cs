using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class Diary
    {
        [Required]
        public virtual int DiaryId { get; set; }
        [Required]
        public virtual AspNetUsers AssignedToUserId { get; set; }
        [Required]
        public virtual PrescriptionNote PrescriptionNote { get; set; }
        [Required]
        public virtual DateTime FollowUpDate { get; set; }
        public virtual DateTime? DateResolved { get; set; }
        [Required]
        public virtual DateTime CreatedDate { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
    }
}