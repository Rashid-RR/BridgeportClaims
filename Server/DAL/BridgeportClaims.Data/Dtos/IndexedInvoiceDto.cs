namespace BridgeportClaims.Data.Dtos
{
    public sealed class IndexedInvoiceDto
    {
        public bool InvoiceNumberIsAlreadyIndexed { get; set; }
        public int? DocumentId { get; set; }
        public string FileUrl { get; set; }
        public string FileName { get; set; }
    }
}