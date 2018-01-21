﻿using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
	public class Episode
	{
		[Required]
	    public virtual int EpisodeId { get; set; }
	    [Required]
        public virtual Claim Claim { get; set; }
	    public virtual EpisodeType EpisodeType { get; set; }
	    public virtual AspNetUsers ResolvedUserId { get; set; }
	    public virtual AspNetUsers AcquiredUserId { get; set; }
        public virtual AspNetUsers AssignedUserId { get; set; }
        [Required]
	    [StringLength(8000)]
	    public virtual string Note { get; set; }
	    [StringLength(25)]
	    public virtual string Role { get; set; }
	    [StringLength(100)]
	    public virtual string RxNumber { get; set; }
	    [StringLength(1)]
	    public virtual string Status { get; set; }
	    public virtual DateTime? CreatedDateUtc { get; set; }
	    [StringLength(255)]
	    public virtual string Description { get; set; }
	    public virtual DateTime? ResolvedDateUtc { get; set; }
	    [Required]
	    public virtual DateTime CreatedOnUtc { get; set; }
	    [Required]
	    public virtual DateTime UpdatedOnUtc { get; set; }
    }
}