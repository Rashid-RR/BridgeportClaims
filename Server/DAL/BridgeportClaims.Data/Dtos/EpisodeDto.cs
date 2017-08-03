using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.Dtos
{
	[Serializable]
	public sealed class EpisodeDto
	{
		public int? EpisodeId { get; set; }
		[Required]
		public int ClaimId { get; set; }
		public DateTime? Date { get; set; }
		public string Type { get; set; }
		public string By { get; set; }
		public string Note { get; set; }
	}
}