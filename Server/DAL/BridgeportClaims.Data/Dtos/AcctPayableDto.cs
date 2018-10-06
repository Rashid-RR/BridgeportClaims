using System;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class AcctPayableDto
    {
        public DateTime? Date { get; set; }
        public string CheckNumber { get; set; }
        public string RxNumber { get; set; }
        public DateTime? RxDate { get; set; }
        public decimal? CheckAmount { get; set; }
    }
}