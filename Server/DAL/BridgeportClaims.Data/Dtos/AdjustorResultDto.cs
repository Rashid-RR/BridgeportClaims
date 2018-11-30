using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class AdjustorResultDto
    {
        [Required]
        public int AdjustorId { get; set; }
        [Required]
        public string AdjustorName { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Extension { get; set; }
        public string ModifiedBy { get; set; }
    }
}
