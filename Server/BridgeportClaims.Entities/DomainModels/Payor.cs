using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BridgeportClaims.Entities.DomainModels
{
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public class Payor
    {
        public Payor()
        {
            Adjustor = new List<Adjustor>();
            Claim = new List<Claim>();
            Invoice = new List<Invoice>();
        }
        public virtual int PayorId { get; set; }
        public virtual UsState UsState { get; set; }
        [Required]
        [StringLength(255)]
        public virtual string BillToName { get; set; }
        [StringLength(255)]
        public virtual string BillToAddress1 { get; set; }
        [StringLength(255)]
        public virtual string BillToAddress2 { get; set; }
        [StringLength(155)]
        public virtual string BillToCity { get; set; }
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
        public virtual DateTime CreatedOn { get; set; }
        [Required]
        public virtual DateTime UpdatedOn { get; set; }
        public virtual IList<Adjustor> Adjustor { get; set; }
        public virtual IList<Claim> Claim { get; set; }
        public virtual IList<Invoice> Invoice { get; set; }
    }
}
