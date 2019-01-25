namespace BridgeportClaims.Web.Models
{
    public sealed class PayorEntityModel
    {
        public int PayorId { get; set; } = 0;
        public string GroupName { get; set; }
        public string BillToName { get; set; }
        public string BillToAddress1 { get; set; }
        public string BillToAddress2 { get; set; }
        public string BillToCity { get; set; }
        public int? BillToStateId { get; set; }
        public string BillToPostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string AlternatePhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string Notes { get; set; }
        public string Contact { get; set; }
        public string LetterName { get; set; }
    }
}