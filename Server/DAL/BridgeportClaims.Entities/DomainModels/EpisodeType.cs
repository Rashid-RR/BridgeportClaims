using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BridgeportClaims.Entities.DomainModels
{
	[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
	public class EpisodeType
	{
		public EpisodeType()
		{
			Episode = new List<Episode>();
		}
		[Required]
		public virtual int EpisodeTypeId { get; set; }
		[Required]
		[StringLength(255)]
		public virtual string TypeName { get; set; }
		[Required]
		[StringLength(10)]
		public virtual string Code { get; set; }
		[StringLength(1000)]
		public virtual string Description { get; set; }
		[Required]
		public virtual DateTime CreatedOnUtc { get; set; }
		[Required]
		public virtual DateTime UpdatedOnUtc { get; set; }
		public virtual IList<Episode> Episode { get; set; }
	}
}