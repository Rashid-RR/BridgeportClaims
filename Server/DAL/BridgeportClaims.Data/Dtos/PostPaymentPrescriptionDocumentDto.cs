namespace BridgeportClaims.Data.Dtos
{
    public sealed class PostPaymentPrescriptionDocumentDto
    {
        public int DocumentId { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public decimal AmountRemaining { get; set; }
    }
}