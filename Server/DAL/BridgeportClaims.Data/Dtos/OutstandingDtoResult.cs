using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class OutstandingDtoResult
    {
        [Required]
        public int PrescriptionId { get; set; }
        [Required]
        public DateTime InvoiceDate { get; set; }
        [Required]
        public string InvoiceNumber { get; set; }
        public string LabelName { get; set; }
        [Required]
        public string BillTo { get; set; }
        [Required]
        public string RxNumber { get; set; }
        [Required]
        public DateTime RxDate { get; set; }
        [Required]
        public decimal InvAmt { get; set; }
        [Required]
        public decimal AmountPaid { get; set; }
        [Required]
        public decimal Outstanding { get; set; }
        public string Status { get; set; }
        [Required]
        public int NoteCount { get; set; }
    }
}
