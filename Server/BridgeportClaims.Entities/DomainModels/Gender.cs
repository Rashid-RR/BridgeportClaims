using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BridgeportClaims.Entities.DomainModels
{
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public class Gender
    {
        public Gender()
        {
            Patient = new List<Patient>();
        }
        public virtual int GenderId { get; set; }
        [Required]
        [StringLength(55)]
        public virtual string GenderName { get; set; }
        [Required]
        [StringLength(5)]
        public virtual string GenderCode { get; set; }
        [Required]
        public virtual DateTime CreatedOn { get; set; }
        [Required]
        public virtual DateTime UpdatedOn { get; set; }
        [Required]
        public virtual DateTime DataVersion { get; set; }
        public virtual IList<Patient> Patient { get; set; }
    }
}