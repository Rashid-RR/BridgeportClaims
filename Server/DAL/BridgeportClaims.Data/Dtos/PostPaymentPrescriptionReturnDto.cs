using System;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public sealed class PostPaymentPrescriptionReturnDto
    {
        public int PrescriptionId { get; set; }
        public decimal Outstanding { get; set; }
    }
}