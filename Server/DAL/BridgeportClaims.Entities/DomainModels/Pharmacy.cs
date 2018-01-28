﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BridgeportClaims.Entities.DomainModels
{
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public class Pharmacy
    {
        public Pharmacy()
        {
            Episode = new List<Episode>();
            Prescription = new List<Prescription>();
        }
        [Required]
        public virtual string Nabp { get; set; }
        public virtual UsState StateId { get; set; }
        [StringLength(10)]
        public virtual string Npi { get; set; }
        [StringLength(60)]
        public virtual string PharmacyName { get; set; }
        [StringLength(55)]
        public virtual string Address1 { get; set; }
        [StringLength(55)]
        public virtual string Address2 { get; set; }
        [StringLength(35)]
        public virtual string City { get; set; }
        [StringLength(11)]
        public virtual string PostalCode { get; set; }
        [StringLength(30)]
        public virtual string PhoneNumber { get; set; }
        [StringLength(30)]
        public virtual string AlternatePhoneNumber { get; set; }
        [StringLength(30)]
        public virtual string FaxNumber { get; set; }
        [StringLength(55)]
        public virtual string Contact { get; set; }
        [StringLength(30)]
        public virtual string ContactPhoneNumber { get; set; }
        [StringLength(50)]
        public virtual string ContactEmailAddress { get; set; }
        [StringLength(15)]
        public virtual string FederalTin { get; set; }
        [StringLength(2)]
        public virtual string DispType { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
        public virtual IList<Episode> Episode { get; set; }
        public virtual IList<Prescription> Prescription { get; set; }
    }
}