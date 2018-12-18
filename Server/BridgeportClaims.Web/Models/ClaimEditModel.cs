using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Web.Models
{
    public sealed class ClaimEditModel
    {
        private const string DefaultString = "NULL";
        private const int DefaultInt = -1;

        [Required]
        public int ClaimId { get; set; }
        public string DateOfBirth { get; set; } = DefaultString;
        [Required]
        public int GenderId { get; set; } = DefaultInt;
        [Required]
        public int PayorId { get; set; } = DefaultInt;
        public int? AdjustorId { get; set; } = DefaultInt;
        public string AdjustorPhone { get; set; } = DefaultString;
        public string DateOfInjury { get; set; } = DefaultString;
        public string AdjustorFax { get; set; } = DefaultString;
        public string Address1 { get; set; } = DefaultString;
        public string Address2 { get; set; } = DefaultString;
        public string City { get; set; } = DefaultString;
        public int? StateId { get; set; } = DefaultInt;
        public string PostalCode { get; set; } = DefaultString;
        public int? ClaimFlex2Id { get; set; } = DefaultInt;
        public string AdjustorExtension { get; set; } = DefaultString;
    }
}