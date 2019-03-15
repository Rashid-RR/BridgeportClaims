using System;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class DecisionTreeChoiceModalHeaderDto
    {
        public int EpisodeId { get; set; }
        public string CreatedBy { get; set; }
        public string EpisodeNote { get; set; }
        public DateTime Created { get; set; }
    }
}