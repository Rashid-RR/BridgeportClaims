using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BridgeportClaims.Entities.DomainModels
{
	[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
	public class AspNetUsers
	{
	    public AspNetUsers()
	    {
	        AspNetUserClaims = new List<AspNetUserClaims>();
	        AspNetUserLogins = new List<AspNetUserLogins>();
	        AspNetUserRoles = new List<AspNetUserRoles>();
	        ClaimNote = new List<ClaimNote>();
	        ClaimsUserHistory = new List<ClaimsUserHistory>();
	        Diary = new List<Diary>();
	        PrescriptionNote = new List<PrescriptionNote>();
	        PrescriptionPayment = new List<PrescriptionPayment>();
	        Suspense = new List<Suspense>();
	    }
        [Required]
	    public virtual string Id { get; set; }
	    [Required]
	    [StringLength(100)]
	    public virtual string FirstName { get; set; }
	    [Required]
	    [StringLength(100)]
	    public virtual string LastName { get; set; }
	    [Required]
	    public virtual DateTime RegisteredDate { get; set; }
	    [Required]
	    [StringLength(256)]
	    public virtual string UserName { get; set; }
	    [StringLength(256)]
	    public virtual string Email { get; set; }
	    [Required]
	    public virtual bool EmailConfirmed { get; set; }
	    [StringLength(4000)]
	    public virtual string PasswordHash { get; set; }
	    [StringLength(4000)]
	    public virtual string SecurityStamp { get; set; }
	    [StringLength(30)]
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
	    public virtual IList<AspNetUserClaims> AspNetUserClaims { get; set; }
	    public virtual IList<AspNetUserLogins> AspNetUserLogins { get; set; }
	    public virtual IList<AspNetUserRoles> AspNetUserRoles { get; set; }
	    public virtual IList<ClaimNote> ClaimNote { get; set; }
	    public virtual IList<ClaimsUserHistory> ClaimsUserHistory { get; set; }
	    public virtual IList<Diary> Diary { get; set; }
	    public virtual IList<PrescriptionNote> PrescriptionNote { get; set; }
	    public virtual IList<PrescriptionPayment> PrescriptionPayment { get; set; }
	    public virtual IList<Suspense> Suspense { get; set; }
    }
}
