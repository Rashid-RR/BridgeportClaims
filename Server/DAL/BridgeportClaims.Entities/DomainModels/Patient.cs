using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BridgeportClaims.Entities.DomainModels
{
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public class Patient
    {
        public Patient()
        {
            Claim = new List<Claim>();
        }
        [Required]
        public virtual int PatientId { get; set; }
        public virtual UsState StateId { get; set; }
        [Required]
        public virtual Gender Gender { get; set; }
        public virtual AspNetUsers ModifiedByUserId { get; set; }
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
        [StringLength(30)]
        public virtual string PhoneNumber { get; set; }
        [StringLength(30)]
        public virtual string AlternatePhoneNumber { get; set; }
        [StringLength(155)]
        public virtual string EmailAddress { get; set; }
        public virtual DateTime? DateOfBirth { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
        public virtual IList<Claim> Claim { get; set; }
    }
}