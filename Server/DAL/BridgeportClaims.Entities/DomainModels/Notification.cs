using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class Notification
    {
        [Required]
        public virtual int NotificationId { get; set; }
        public virtual AspNetUsers DismissedByUser { get; set; }
        [Required]
        public virtual NotificationType NotificationType { get; set; }
        [Required]
        [StringLength(4000)]
        public virtual string MessageText { get; set; }
        [Required]
        public virtual DateTime GeneratedDate { get; set; }
        [Required]
        public virtual bool IsDismissed { get; set; }
        public virtual DateTime? DismissedDate { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
    }
}