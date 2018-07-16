namespace BridgeportClaims.Data.Dtos
{
	public sealed class EpisodeTypeDto
	{
		public int EpisodeTypeId { get; set; }
		public string EpisodeTypeName { get; set; }
        public byte SortOrder { get; set; }
	}
}