using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class LetterGenerationDto
    {
        public DateTime TodaysDate { get; set; }
        [Required]
        [StringLength(155)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(155)]
        public string LastName { get; set; }
        [StringLength(255)]
        public string Address1 { get; set; }
        [StringLength(255)]
        public string Address2 { get; set; }
        [StringLength(155)]
        public string City { get; set; }
        [StringLength(2)]
        public string StateCode { get; set; }
        [StringLength(100)]
        public string PostalCode { get; set; }
        [Required]
        [StringLength(255)]
        public string LetterName { get; set; }
        [StringLength(255)]
        public string BillToName { get; set; }
        [StringLength(100)]
        public string UserFirstName { get; set; }
        [StringLength(100)]
        public string UserLastName { get; set; }
        [StringLength(60)]
        public string PharmacyName { get; set; }
        [StringLength(30)]
        public string Extension { get; set; }
        [StringLength(255)]
        public string AttorneyName { get; set; }
        [StringLength(255)]
        public string AttorneyAddress1 { get; set; }
        [StringLength(255)]
        public string AttorneyAddress2 { get; set; }
        [StringLength(255)]
        public string AttorneyCity { get; set; }
        [StringLength(2)]
        public string AttorneyStateCode { get; set; }
        [StringLength(255)]
        public string AttorneyPostalCode { get; set; }
        [Required]
        [StringLength(255)]
        public string ClaimNumber { get; set; }
    }
}