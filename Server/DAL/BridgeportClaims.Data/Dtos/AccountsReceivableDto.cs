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
        public decimal Jan17 { get; set; }
        [Required]
        public decimal Feb17 { get; set; }
        [Required]
        public decimal Mar17 { get; set; }
        [Required]
        public decimal Apr17 { get; set; }
        [Required]
        public decimal May17 { get; set; }
        [Required]
        public decimal Jun17 { get; set; }
        [Required]
        public decimal Jul17 { get; set; }
        [Required]
        public decimal Aug17 { get; set; }
        [Required]
        public decimal Sep17 { get; set; }
        [Required]
        public decimal Oct17 { get; set; }
        [Required]
        public decimal Nov17 { get; set; }
        [Required]
        public decimal Dec17 { get; set; }
    }
}