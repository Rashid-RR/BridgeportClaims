using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BridgeportClaims.Entities.DomainModels
{
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public class AspNetRoles
    {
        public AspNetRoles()
        {
            AspNetUserRoles = new List<AspNetUserRoles>();
        }
        public virtual string Id { get; set; }
        [Required]
        [StringLength(256)]
        public virtual string Name { get; set; }
        public virtual IList<AspNetUserRoles> AspNetUserRoles { get; set; }
    }
}
