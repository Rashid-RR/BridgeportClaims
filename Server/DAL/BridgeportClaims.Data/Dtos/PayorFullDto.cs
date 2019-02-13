using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class PayorFullDto
    {
        [Required]
        public int PayorId { get; set; }
        [Required]
        [StringLength(255)]
        public string GroupName { get; set; }
        [StringLength(255)]
        [Required]
        public string BillToName { get; set; }
        [StringLength(255)]
        public string BillToAddress1 { get; set; }
        [StringLength(255)]
        public string BillToAddress2 { get; set; }
        [StringLength(155)]
        public string BillToCity { get; set; }
        [StringLength(2)]
        public string State { get; set; }
        [StringLength(100)]
        public string BillToPostalCode { get; set; }
        [StringLength(30)]
        public string PhoneNumber { get; set; }
        [StringLength(30)]
        public string AlternatePhoneNumber { get; set; }
        [StringLength(30)]
        public string FaxNumber { get; set; }
        [StringLength(8000)]
        public string Notes { get; set; }
        [StringLength(255)]
        public string Contact { get; set; }
        [Required]
        public string LetterName { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]
        public DateTime UpdatedOn { get; set; }
    }
}