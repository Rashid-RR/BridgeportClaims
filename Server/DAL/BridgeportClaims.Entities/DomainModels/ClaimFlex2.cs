using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BridgeportClaims.Entities.DomainModels
{
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public class ClaimFlex2
    {
        public ClaimFlex2()
        {
            Claim = new List<Claim>();
        }
        [Required]
        public virtual int ClaimFlex2Id { get; set; }
        [Required]
        [StringLength(10)]
        public virtual string Flex2 { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
        public virtual IList<Claim> Claim { get; set; }
    }
}