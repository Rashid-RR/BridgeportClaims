using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class EpisodeLink
    {
        [Required]
        public virtual int EpisodeLinkId { get; set; }
        public virtual EpisodeLinkType EpisodeLinkType { get; set; }
        [StringLength(50)]
        public virtual string LinkTransNumber { get; set; }
        public virtual int? EpisodeNumber { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
    }
}