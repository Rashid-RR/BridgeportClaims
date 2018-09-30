namespace BridgeportClaims.Data.Dtos
{
    public sealed class PostPaymentPrescriptionReturnDto
    {
        public int PrescriptionId { get; set; }
        public decimal Outstanding { get; set; }
    }
}