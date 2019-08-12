using System;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class InvoiceProcessDto
    {
        private DateTime _rxDate;
        public string RxDate
        {
            get => $"{_rxDate:MM/dd/yyyy}";
            set => _rxDate = Convert.ToDateTime(value);
        }
        public string Carrier { get; set; }
        public string PatientName { get; set; }
        public string ClaimNumber { get; set; }
        public int InQueue { get; set; }
    }
}