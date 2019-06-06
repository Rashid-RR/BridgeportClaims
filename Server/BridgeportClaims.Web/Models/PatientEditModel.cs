using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Web.Models
{
    public sealed class PatientEditModel
    {
        private const string DefaultString = "{NULL}";
        private const int DefaultInt = -1;
        [Required]
        public int PatientId { get; set; }
        public string LastName { get; set; } = DefaultString;
        public string FirstName { get; set; } = DefaultString;
        public string Address1 { get; set; } = DefaultString;
        public string Address2 { get; set; } = DefaultString;
        public string City { get; set; } = DefaultString;
        public string PostalCode { get; set; } = DefaultString;
        public int? StateId { get; set; } = DefaultInt;
        public string StateName { get; set; } = DefaultString;
        public string PhoneNumber { get; set; } = DefaultString;
        public string EmailAddress { get; set; } = DefaultString;
    }
}