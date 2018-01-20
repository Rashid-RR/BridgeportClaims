using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BridgeportClaims.Entities.DomainModels
{
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public class PrescriptionStatus
    {
        public PrescriptionStatus()
        {
            Prescription = new List<Prescription>();
        }
        [Required]
        public virtual int PrescriptionStatusId { get; set; }
        [Required]
        [StringLength(100)]
        public virtual string StatusName { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
        public virtual IList<Prescription> Prescription { get; set; }
    }
}