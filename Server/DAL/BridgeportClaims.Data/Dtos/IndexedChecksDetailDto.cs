using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class IndexedChecksDetailDto
    {
        [Required]
        public int PrescriptionPaymentId { get; set; }
        [Required]
        public string CheckNumber { get; set; }
        [Required]
        public decimal AmountPaid { get; set; }
        public DateTime? DatePosted { get; set; }
        [Required]
        public string IndexedBy { get; set; }
        [Required]
        public string RxNumber { get; set; }
        [Required]
        public DateTime RxDate { get; set; }
    }
}