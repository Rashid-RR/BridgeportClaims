using System.Threading.Tasks;

namespace BridgeportClaims.RedisCache.Clearing
{
    public interface ICachingClearingService
    {
        Task ClearClaimNoteTypeCache();
        Task ClearClaimUserHistoryCache(string userId);
    }
}