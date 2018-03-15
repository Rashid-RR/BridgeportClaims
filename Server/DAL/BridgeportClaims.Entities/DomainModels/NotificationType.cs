using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BridgeportClaims.Entities.DomainModels
{
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public class NotificationType
    {
        public NotificationType()
        {
            Notification = new List<Notification>();
            NotificationConfig = new List<NotificationConfig>();
        }
        [Required]
        public virtual byte NotificationTypeId { get; set; }
        [Required]
        [StringLength(255)]
        public virtual string TypeName { get; set; }
        [Required]
        [StringLength(50)]
        public virtual string Code { get; set; }
        [StringLength(1000)]
        public virtual string NotificationConfigDescription { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
        public virtual IList<Notification> Notification { get; set; }
        public virtual IList<NotificationConfig> NotificationConfig { get; set; }
    }
}