namespace PdfGeneratorApi.Common.Models
{
    public sealed class InvoicePrescriptionPdfModel
    {
        public int PrescriptionId { get; set; }
        public string Prescriber { get; set; }
        public string PrescriberNpi { get; set; }
        public string DateFilledDay { get; set; }
        public string DateFilledMonth { get; set; }
        public string DateFilledYear { get; set; }
        public string Ndc { get; set; }
        public string LabelName { get; set; }
        public string RxNumber { get; set; }
        public decimal BilledAmount { get; set; }
        public int? BilledAmountDollars { get; set; }
        public string BilledAmountCents { get; set; }
        public float Quantity { get; set; }
        public string PharmacyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string PharmacyState { get; set; }
        public string PostalCode { get; set; }
        public string FederalTin { get; set; }
        public string Npi { get; set; }
        public string Nabp { get; set; }
    }
}