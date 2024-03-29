﻿using BridgeportClaims.RedisCache.Keys.Abstractions;

namespace BridgeportClaims.Tests.RedisCache.Keys
{
    public class FakeCacheKey : AbstractCacheKey
    {
        public const string KeyFormat = "{{PersonCacheKey_v1}}";
        public override string CacheKey => KeyFormat;
    }
}