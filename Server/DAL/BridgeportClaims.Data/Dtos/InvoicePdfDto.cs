namespace BridgeportClaims.Data.Dtos
{
    public sealed class InvoicePdfDto
    {
        public string BillToName { get; set; }
        public string BillToAddress1 { get; set; }
        public string BillToAddress2 { get; set; }
        public string BillToCity { get; set; }
        public string BillToStateCode { get; set; }
        public string BillToPostalCode { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string ClaimNumber { get; set; }
        public string PatientLastName { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientAddress1 { get; set; }
        public string PatientAddress2 { get; set; }
        public string PatientCity { get; set; }
        public string PatientStateCode { get; set; }
        public string PatientPostalCode { get; set; }
        public string PatientPhoneNumber { get; set; }
        public string DateOfBirthDay { get; set; }
        public string DateOfBirthMonth { get; set; }
        public string DateOfBirthYear { get; set; }
        public bool? IsMale { get; set; }
        public bool? IsFemale { get; set; }
        public string DateOfInjuryDay { get; set; }
        public string DateOfInjuryMonth { get; set; }
        public string DateOfInjuryYear { get; set; }
        public string Prescriber { get; set; }
        public string PrescriberNpi { get; set; }
        public string DateFilledDay { get; set; }
        public string DateFilledMonth { get; set; }
        public string DateFilledYear { get; set; }
        public string Ndc { get; set; }
        public string LabelName { get; set; }
        public string RxNumber { get; set; }
        public decimal BilledAmount { get; set; }
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