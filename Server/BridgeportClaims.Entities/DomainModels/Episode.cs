﻿using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class Episode
    {
        public virtual int EpisodeId { get; set; }
        public virtual Claim Claim { get; set; }
        public virtual int? EpisodeNumber { get; set; }
        [StringLength(50)]
        public virtual string Note { get; set; }
        [StringLength(50)]
        public virtual string Role { get; set; }
        [StringLength(50)]
        public virtual string Type { get; set; }
        [StringLength(100)]
        public virtual string ResolvedUser { get; set; }
        [StringLength(100)]
        public virtual string AcquiredUser { get; set; }
        [StringLength(100)]
        public virtual string AssignUser { get; set; }
        [StringLength(100)]
        public virtual string RxNumber { get; set; }
        [StringLength(1)]
        public virtual string Status { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        [StringLength(255)]
        public virtual string Description { get; set; }
        public virtual DateTime? ResolvedDate { get; set; }
        [Required]
        public virtual DateTime CreatedOn { get; set; }
        [Required]
        public virtual DateTime UpdatedOn { get; set; }
    }
}