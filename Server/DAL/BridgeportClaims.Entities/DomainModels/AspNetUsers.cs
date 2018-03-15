using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
	public class AspNetUsers
	{
		public AspNetUsers()
		{
			Adjustor = new List<Adjustor>();
			AspNetUserClaims = new List<AspNetUserClaims>();
			AspNetUserLogins = new List<AspNetUserLogins>();
			AspNetUserRoles = new List<AspNetUserRoles>();
			Claim = new List<Claim>();
			ClaimNote = new List<ClaimNote>();
			ClaimsUserHistory = new List<ClaimsUserHistory>();
			Diary = new List<Diary>();
			Document = new List<Document>();
			DocumentIndex = new List<DocumentIndex>();
			AcquiredUser = new List<Episode>();
			AssignedUser = new List<Episode>();
			ResolvedUser = new List<Episode>();
			ModifiedByUser = new List<Episode>();
			EpisodeNote = new List<EpisodeNote>();
			EpisodeTypeUsersMapping = new List<EpisodeTypeUsersMapping>();
			InvoiceIndex = new List<InvoiceIndex>();
			Notification = new List<Notification>();
			Patient = new List<Patient>();
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
		[StringLength(30)]
		public virtual string Extension { get; set; }
		public virtual IList<Adjustor> Adjustor { get; set; }
		public virtual IList<AspNetUserClaims> AspNetUserClaims { get; set; }
		public virtual IList<AspNetUserLogins> AspNetUserLogins { get; set; }
		public virtual IList<AspNetUserRoles> AspNetUserRoles { get; set; }
		public virtual IList<Claim> Claim { get; set; }
		public virtual IList<ClaimNote> ClaimNote { get; set; }
		public virtual IList<ClaimsUserHistory> ClaimsUserHistory { get; set; }
		public virtual IList<Diary> Diary { get; set; }
		public virtual IList<Document> Document { get; set; }
		public virtual IList<DocumentIndex> DocumentIndex { get; set; }
		public virtual IList<Episode> AcquiredUser { get; set; }
		public virtual IList<Episode> AssignedUser { get; set; }
		public virtual IList<Episode> ResolvedUser { get; set; }
		public virtual IList<Episode> ModifiedByUser { get; set; }
		public virtual IList<EpisodeNote> EpisodeNote { get; set; }
		public virtual IList<EpisodeTypeUsersMapping> EpisodeTypeUsersMapping { get; set; }
		public virtual IList<InvoiceIndex> InvoiceIndex { get; set; }
		public virtual IList<Notification> Notification { get; set; }
		public virtual IList<Patient> Patient { get; set; }
		public virtual IList<PrescriptionNote> PrescriptionNote { get; set; }
		public virtual IList<PrescriptionPayment> PrescriptionPayment { get; set; }
		public virtual IList<Suspense> Suspense { get; set; }
	}
}
