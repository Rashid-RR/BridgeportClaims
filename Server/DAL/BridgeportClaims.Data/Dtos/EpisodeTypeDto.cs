using System;

namespace BridgeportClaims.Data.Dtos
{
	[Serializable]
	public sealed class EpisodeTypeDto
	{
		public int EpisodeTypeId { get; set; }
		public string EpisodeTypeName { get; set; }
        public byte SortOrder { get; set; }
	}
}