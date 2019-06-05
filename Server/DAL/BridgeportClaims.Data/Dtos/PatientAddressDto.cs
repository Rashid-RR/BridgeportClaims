namespace BridgeportClaims.Data.Dtos
{
    public class PatientAddressDto
    {
        public int PatientId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public int? StateId { get; set; }
        public string StateCode { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
    }
}
