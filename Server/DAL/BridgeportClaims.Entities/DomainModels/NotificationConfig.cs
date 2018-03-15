using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class NotificationConfig
    {
        [Required]
        public virtual int NotificationConfigId { get; set; }
        [Required]
        public virtual NotificationType NotificationType { get; set; }
        [Required]
        public virtual string NotificationValue { get; set; }
        [Required]
        [StringLength(30)]
        public virtual string SqlVariantDataType { get; set; }
        [Required]
        public virtual DateTime EffectiveDate { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
    }
}