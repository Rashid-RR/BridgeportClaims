using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    public class EpisodesDto
    {
        public int TotalRowCount { get; set; }
        public IList<EpisodeResultsDto> EpisodeResults { get; set; }
    }
}