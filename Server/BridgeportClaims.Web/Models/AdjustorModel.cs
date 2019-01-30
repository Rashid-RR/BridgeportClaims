using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Web.Models
{
    public class AdjustorModel
    {
        [Required]
        public int AdjustorId { get; set; } = 0;
        [Required]
        public string AdjustorName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public int? StateId { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Extension { get; set; }
    }
}