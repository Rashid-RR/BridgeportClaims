using System.Collections.Generic;

namespace PdfGeneratorApi.Common.Models
{
    public sealed class InvoicePdfModel
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
        public IList<InvoicePrescriptionPdfModel> Scripts { get; set; }
    }
}