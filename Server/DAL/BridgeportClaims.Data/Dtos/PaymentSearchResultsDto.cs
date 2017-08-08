using System;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public sealed class PaymentSearchResultsDto
    {
        public string ClaimNumber { get; set; }
        public string PatientName { get; set; }
        public string Payor { get; set; }
        public int NumberOfPrescriptions { get; set; }
    }
}
