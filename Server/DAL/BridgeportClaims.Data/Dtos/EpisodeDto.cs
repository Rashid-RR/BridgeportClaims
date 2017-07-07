using System;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public sealed class EpisodeDto
    {
        public int? EpisodeId { get; set; }
        public int ClaimId { get; set; }
        public DateTime? Date { get; set; }
        public string By { get; set; }
        public string Note { get; set; }
    }
}