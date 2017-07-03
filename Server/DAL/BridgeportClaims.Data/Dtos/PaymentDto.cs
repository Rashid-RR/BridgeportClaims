using System;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public sealed class PaymentDto
    {
        public DateTime? Date { get; set; }
        public string CheckNumber { get; set; }
        public string RxNumber { get; set; }
        public DateTime? RxDate { get; set; }
        public decimal? CheckAmount { get; set; }
    }
}