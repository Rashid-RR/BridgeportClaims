using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.ViewModels
{
    public class PayorViewModel
    {
        [Required]
        public int PayorId { get; set; }
        [Required]
        [StringLength(255)]
        public virtual string BillToName { get; set; }
        [StringLength(255)]
        public virtual string BillToAddress1 { get; set; }
        [StringLength(255)]
        public virtual string BillToAddress2 { get; set; }
        [StringLength(155)]
        public virtual string BillToCity { get; set; }
        [StringLength(2)]
        public string State { get; set; }
        [StringLength(100)]
        public virtual string BillToPostalCode { get; set; }
        [StringLength(30)]
        public virtual string PhoneNumber { get; set; }
        [StringLength(30)]
        public virtual string AlternatePhoneNumber { get; set; }
        [StringLength(30)]
        public virtual string FaxNumber { get; set; }
        [StringLength(8000)]
        public virtual string Notes { get; set; }
        [StringLength(255)]
        public virtual string Contact { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]
        public DateTime UpdatedOn { get; set; }
    }
}