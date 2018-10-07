using System;
using System.Threading.Tasks;
using BridgeportClaims.RedisCache.Domain;
using BridgeportClaims.RedisCache.Keys;
using BridgeportClaims.RedisCache.Keys.Abstractions;

namespace BridgeportClaims.RedisCache.Clearing
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

        public async Task ClearClaimUserHistoryCache(string userId)
        {
            ICacheKey cacheKey = new ClaimUserHistoryCacheKey(userId);
            await _redisDomain.Value.RemoveAsync(cacheKey).ConfigureAwait(false);
        }
    }
}