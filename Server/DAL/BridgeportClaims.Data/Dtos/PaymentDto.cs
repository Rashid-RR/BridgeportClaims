using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public class PaymentDto
    {
        [Required]
        public string ClaimNumber { get; set; }
        public string PatientName { get; set; }
        public string RxDate { get; set; }
        public string RxNumber { get; set; }
        public string LabelName { get; set; }
        public decimal? InvoicedAmount { get; set; }
        public decimal? Outstanding { get; set; }
        public string Payor { get; set; }
    }
}
