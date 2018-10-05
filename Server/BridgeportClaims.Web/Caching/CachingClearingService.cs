using BridgeportClaims.RedisCache.Domain;
using BridgeportClaims.RedisCache.Keys;
using System;
using System.Threading.Tasks;

namespace BridgeportClaims.Web.Caching
{
    public class CachingClearingService : ICachingClearingService
    {
        private readonly Lazy<IRedisDomain> _redisDomain;

        public CachingClearingService(Lazy<IRedisDomain> redisDomain)
        {
            _redisDomain = redisDomain;
        }

        public async Task ClearClaimNoteTypeCache()
        {
            ICacheKey cacheKey = new ClaimNoteTypeCacheKey();
            await _redisDomain.Value.RemoveAsync(cacheKey).ConfigureAwait(false);
        }

        private string DecorateKey(string cacheKey)
        {
            if (true) // TODO: Figure out if this is Prod.
            {
                return cacheKey;
            }
            return cacheKey + System.Environment.MachineName;
        }
    }
}