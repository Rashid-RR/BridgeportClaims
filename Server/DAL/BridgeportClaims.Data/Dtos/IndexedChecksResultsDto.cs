using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class IndexedChecksResultsDto
    {
        [Required]
        public int DocumentId { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public string CheckNumber { get; set; }
        [Required]
        public string IndexedBy { get; set; }
        [Required]
        public string FileUrl { get; set; }
        public long? NumberOfPayments { get; set; }
        public decimal? TotalAmountPaid { get; set; }
        [Required]
        public DateTime IndexedOn { get; set; }
    }
}