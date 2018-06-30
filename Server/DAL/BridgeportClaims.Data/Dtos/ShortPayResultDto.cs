using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class ShortPayResultDto
    {
        [Required]
        public int PrescriptionPaymentId { get; set; }
        [Required]
        public string RxNumber { get; set; }
        public string RxDate { get; set; }
        [Required]
        public decimal BilledAmount { get; set; }
        [Required]
        public decimal AmountPaid { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string ClaimNumber { get; set; }
        public string PrescriptionStatus { get; set; }
    }
}