using System;

namespace BridgeportClaims.Data.Dtos
{
    public class EpisodeDto
    {
        public DateTime? Date { get; set; }
        public string EnteredBy { get; set; }
        public string Note { get; set; }
    }
}