using System;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class PrescriptionDto
    {
        public int PrescriptionId { get; set; }
        public DateTime? RxDate { get; set; }
        public string RxNumber { get; set; }
        public string LabelName { get; set; }
        public string BillTo { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public decimal? AmountPaid { get; set; }
        public decimal? Outstanding { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string Status { get; set; }
        public int NoteCount { get; set; }
        public bool IsReversed { get; set; }
        public string Prescriber { get; set; }
        public string PrescriberNpi { get; set; }
        public string PharmacyName { get; set; }
        public string PrescriptionNdc { get; set; }
        public string PrescriberPhone { get; set; }
        public bool InvoiceIsIndexed { get; set; }
        public string InvoiceUrl { get; set; }
    }
}