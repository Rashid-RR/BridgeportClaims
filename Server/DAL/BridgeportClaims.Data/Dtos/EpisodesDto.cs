using System;
using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public class EpisodesDto
    {
        public int TotalRowCount { get; set; }
        public IList<EpisodeResultsDto> EpisodeResults { get; set; }
    }
}