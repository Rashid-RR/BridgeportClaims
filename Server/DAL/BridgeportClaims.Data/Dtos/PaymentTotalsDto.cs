using System;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class PaymentTotalsDto
    {
        public DateTime DatePosted { get; set; }
        public decimal TotalPosted { get; set; }
    }
}