using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class Patient
    {
        public virtual int PatientId { get; set; }
        public virtual Claim Claim { get; set; }
        public virtual UsState UsState { get; set; }
        public virtual Gender Gender { get; set; }
        [Required]
        [StringLength(155)]
        public virtual string LastName { get; set; }
        [Required]
        [StringLength(155)]
        public virtual string FirstName { get; set; }
        [StringLength(255)]
        public virtual string Address1 { get; set; }
        [StringLength(255)]
        public virtual string Address2 { get; set; }
        [StringLength(155)]
        public virtual string City { get; set; }
        [StringLength(100)]
        public virtual string PostalCode { get; set; }
        [StringLength(2)]
        public virtual string StateCode { get; set; }
        [StringLength(30)]
        public virtual string PhoneNumber { get; set; }
        [StringLength(30)]
        public virtual string AlternatePhoneNumber { get; set; }
        [StringLength(155)]
        public virtual string EmailAddress { get; set; }
        [Required]
        public virtual DateTime DateOfBirth { get; set; }
        [Required]
        public virtual DateTime CreatedOn { get; set; }
        [Required]
        public virtual DateTime UpdatedOn { get; set; }
        [Required]
        public virtual DateTime DataVersion { get; set; }
    }
}