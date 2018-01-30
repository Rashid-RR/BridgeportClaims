using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BridgeportClaims.Entities.DomainModels
{
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public class EpisodeCategory
    {
        public EpisodeCategory()
        {
            Episode = new List<Episode>();
        }
        public virtual int EpisodeCategoryId { get; set; }
        [Required]
        [StringLength(255)]
        public virtual string CategoryName { get; set; }
        [Required]
        [StringLength(50)]
        public virtual string Code { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
        [Required]
        public virtual DateTime DataVersion { get; set; }
        public virtual IList<Episode> Episode { get; set; }
    }
}