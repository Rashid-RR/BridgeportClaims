using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.EpisodeNotes
{
    public interface IEpisodeNoteProvider
    {
        IList<EpisodeNotesDto> GetEpisodeNotes(int episodeId);
    }
}