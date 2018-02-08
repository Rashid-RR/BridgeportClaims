using System;

namespace BridgeportClaims.Web.Models
{
    [Serializable]
    public sealed class SaveEpisodeNoteModel
    {
        public int EpisodeId { get; set; }
        public string Note { get; set; }
    }
}