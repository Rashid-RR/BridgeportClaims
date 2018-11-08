using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class LetterGenerationDto
    {
        [StringLength(4000)]
        public string TodaysDate { get; set; }
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
        [StringLength(100)]
        public string UserFirstName { get; set; }
        [StringLength(100)]
        public string UserLastName { get; set; }
        [StringLength(60)]
        public string PharmacyName { get; set; }
        [StringLength(30)]
        public string Extension { get; set; }
    }
}