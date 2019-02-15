using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class AdjustorFullDto
    {
        [Required]
        public int AdjustorId { get; set; }
        [Required]
        public string AdjustorName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string StateName { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Extension { get; set; }
        public string ModifiedBy { get; set; }
    }
}