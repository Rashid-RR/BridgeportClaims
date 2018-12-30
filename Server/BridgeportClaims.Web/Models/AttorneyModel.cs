namespace BridgeportClaims.Web.Models
{
    public sealed class AttorneyModel
    {
        public int AttorneyId { get; set; } = 0;
        public string AttorneyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
    }
}