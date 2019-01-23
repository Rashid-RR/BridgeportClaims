using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class PayorResultDto
    {
        [Required]
        public int PayorId { get; set; }
        [Required]
        public string GroupName { get; set; }
        public string BillingAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string AlternatePhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string Notes { get; set; }
        public string Contact { get; set; }
        [Required]
        public string LetterName { get; set; }
        public string ModifiedBy { get; set; }
    }
}