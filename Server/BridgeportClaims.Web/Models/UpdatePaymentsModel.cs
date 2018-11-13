using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Web.Models
{
    public sealed class UpdatePaymentsModel
    {
        [Required]
        public int PrescriptionPaymentId { get; set; }
        [Required]
        public string CheckNumber { get; set; }
        [Required]
        public decimal AmountPaid { get; set; }
        public string DatePosted { get; set; }
        [Required]
        public string IndexedBy { get; set; }
        [Required]
        public string RxNumber { get; set; }
        [Required]
        public string RxDate { get; set; }
    }
}