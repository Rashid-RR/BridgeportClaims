using System;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class InvoiceProcessDto
    {
        private DateTime _dateSubmitted;
        public string DateSubmitted
        {
            get => $"{_dateSubmitted:MM/dd/yyyy}";
            set => _dateSubmitted = Convert.ToDateTime(value);
        }
        public string Carrier { get; set; }
        public string PatientName { get; set; }
        public string ClaimNumber { get; set; }
        public int InQueue { get; set; }
    }
}