using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.ViewModels
{
    public class PayorViewModel
    {
        public int PayorId { get; set; }
        public string BillToName { get; set; }
        public string BillToAddress1 { get; set; }
        public string BillToAddress2 { get; set; }
        public string BillToCity { get; set; }
        public string State { get; set; }
        public string BillToPostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string AlternatePhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string Notes { get; set; }
        public string Contact { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]
        public DateTime UpdatedOn { get; set; }
    }
}