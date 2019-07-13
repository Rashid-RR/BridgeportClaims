using System;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class NotificationDto
    {
        public int NotificationId { get; set; }
        public string MessageText { get; set; }
        public DateTime GeneratedDate { get; set; }
        public string NotificationType { get; set; }
        public int? PrescriptionId { get; set; }
        public bool NeedsCarrier { get; set; }
        public int? ClaimId { get; set; }
    }
}