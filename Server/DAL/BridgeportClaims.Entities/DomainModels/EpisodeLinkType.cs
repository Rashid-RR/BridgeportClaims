using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BridgeportClaims.Entities.DomainModels
{
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public class EpisodeLinkType
    {
        public EpisodeLinkType() { EpisodeLink = new List<EpisodeLink>(); }
        [Required]
        public virtual int EpisodeLinkTypeId { get; set; }
        [Required]
        [StringLength(50)]
        public virtual string EpisodeLinkName { get; set; }
        [Required]
        [StringLength(10)]
        public virtual string EpisodeLinkCode { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
        public virtual IList<EpisodeLink> EpisodeLink { get; set; }
    }
}