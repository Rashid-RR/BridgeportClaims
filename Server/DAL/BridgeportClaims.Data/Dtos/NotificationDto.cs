using System;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class NotificationDto
    {
        public int Notificationid { get; set; }
        public string Messagetext { get; set; }
        public DateTime Generateddate { get; set; }
        public string Notificationtype { get; set; }
    }
}