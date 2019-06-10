namespace BridgeportClaims.Data.Dtos
{
    public sealed class InvoiceDto
    {
        public string InvoiceDate { get; set; }
        public string Carrier { get; set; }
        public string PatientName { get; set; }
        public string ClaimNumber { get; set; }
        public int InvoiceCount { get; set; }
        public int ScriptCount { get; set; }
        public int Printed { get; set; }
        public int TotalToPrint { get; set; }
    }
}