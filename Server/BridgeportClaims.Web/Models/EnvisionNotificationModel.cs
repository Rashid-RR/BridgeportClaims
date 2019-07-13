namespace BridgeportClaims.Web.Models
{
    public sealed class EnvisionNotificationModel
    {
        public int PrescriptionId { get; set; }
        public decimal BilledAmount { get; set; }
        public int? PayorId { get; set; }
    }
}