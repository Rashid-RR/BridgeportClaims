using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{

    public class AspNetUsers
    {
        public AspNetUsers()
        {
            AspNetUserClaims = new List<AspNetUserClaims>();
            AspNetUserLogins = new List<AspNetUserLogins>();
            AspNetUserRoles = new List<AspNetUserRoles>();
            ClaimNote = new List<ClaimNote>();
            PrescriptionNote = new List<PrescriptionNote>();
        }
        public virtual string Id { get; set; }
        [StringLength(256)]
        public virtual string Email { get; set; }
        [Required]
        public virtual bool EmailConfirmed { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }
        public virtual string PhoneNumber { get; set; }
        [Required]
        public virtual bool PhoneNumberConfirmed { get; set; }
        [Required]
        public virtual bool TwoFactorEnabled { get; set; }
        public virtual DateTime? LockoutEndDateUtc { get; set; }
        [Required]
        public virtual bool LockoutEnabled { get; set; }
        [Required]
        public virtual int AccessFailedCount { get; set; }
        [Required]
        [StringLength(256)]
        public virtual string UserName { get; set; }
        public virtual IList<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual IList<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual IList<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual IList<ClaimNote> ClaimNote { get; set; }
        public virtual IList<PrescriptionNote> PrescriptionNote { get; set; }
    }
}
