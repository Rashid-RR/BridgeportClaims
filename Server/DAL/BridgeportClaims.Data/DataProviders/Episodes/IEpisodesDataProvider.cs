using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Episodes
{
    public interface IEpisodesDataProvider
    {
        void AddOrUpdateEpisode(EpisodeDto episode);
    }
}