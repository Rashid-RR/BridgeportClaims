using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BridgeportClaims.Entities.DomainModels
{
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public class ClaimNoteType
    {
        public ClaimNoteType()
        {
            ClaimNote = new List<ClaimNote>();
        }
        public virtual int ClaimNoteTypeId { get; set; }
        [Required]
        [StringLength(255)]
        public virtual string TypeName { get; set; }
        [Required]
        [StringLength(10)]
        public virtual string Code { get; set; }
        [Required]
        public virtual DateTime CreatedOn { get; set; }
        [Required]
        public virtual DateTime UpdatedOn { get; set; }
        [Required]
        public virtual DateTime DataVersion { get; set; }
        public virtual IList<ClaimNote> ClaimNote { get; set; }
    }
}
