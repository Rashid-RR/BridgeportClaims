using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class DrNoteLetterGenerationResultsDto
    {
        public DateTime TodaysDate { get; set; }
        public string PrescriberName { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string PostalCode { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [Required]
        public string ClaimNumber { get; set; }
        [Required]
        public string LetterName { get; set; }
        public string BillToName { get; set; }
        [Required]
        public string Plurality { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string Extension { get; set; }
        public string PharmacyName { get; set; }
    }
}