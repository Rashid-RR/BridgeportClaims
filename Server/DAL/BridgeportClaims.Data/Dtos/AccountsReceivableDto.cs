using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public sealed class AccountsReceivableDto
    {
        [Required]
        [StringLength(10)]
        public string MonthBilled { get; set; }
        [Required]
        public byte YearBilled { get; set; }
        [Required]
        public decimal TotalInvoiced { get; set; }
        [Required]
        public decimal Mnth1 { get; set; }
        [Required]
        public decimal Mnth2 { get; set; }
        [Required]
        public decimal Mnth3 { get; set; }
        [Required]
        public decimal Mnth4 { get; set; }
        [Required]
        public decimal Mnth5 { get; set; }
        [Required]
        public decimal Mnth6 { get; set; }
        [Required]
        public decimal Mnth7 { get; set; }
        [Required]
        public decimal Mnth8 { get; set; }
        [Required]
        public decimal Mnth9 { get; set; }
        [Required]
        public decimal Mnth10 { get; set; }
        [Required]
        public decimal Mnth11 { get; set; }
        [Required]
        public decimal Mnth12 { get; set; }
    }
}