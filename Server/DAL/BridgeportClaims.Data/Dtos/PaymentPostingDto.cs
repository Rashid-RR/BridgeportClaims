using System.Diagnostics.CodeAnalysis;

namespace BridgeportClaims.Data.Dtos
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public sealed class PaymentPostingDto
    {
        public int PrescriptionID { get; set; }
        public decimal AmountPosted { get; set; }
    }
}