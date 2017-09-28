using System;

namespace BridgeportClaims.Data.Dtos
{
	[Serializable]
	public sealed class EpisodeDto
	{
		public int? EpisodeId { get; set; }
		public DateTime? Date { get; set; }
		public string Role { get; set; }
		public string By { get; set; }
		public string Note { get; set; }
	}
}