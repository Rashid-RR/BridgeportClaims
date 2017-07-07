using System.Threading.Tasks;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Episodes
{
    public interface IEpisodesDataProvider
    {
        Task AddOrUpdateEpisode(EpisodeDto episode);
    }
}