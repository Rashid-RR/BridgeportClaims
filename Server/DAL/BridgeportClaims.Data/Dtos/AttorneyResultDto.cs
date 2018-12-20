using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class AttorneyResultDto
    {
        [Required]
        public int AttorneyId { get; set; }
        [Required]
        public string AttorneyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string StateName { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        [Required]
        public string ModifiedBy { get; set; }
    }
}
}