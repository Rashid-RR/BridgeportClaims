﻿using System;
using BridgeportClaims.RedisCache.Keys.Abstractions;

namespace BridgeportClaims.RedisCache.Keys
{
    public sealed class QueryBuilderCacheKey : AbstractCacheKey
    {
        private const int SevenHours = 7;
        public const string KeyFormat = "{{QueryBuilderCacheKey_v1}}";
        public override TimeSpan RedisExpirationTimespan => new TimeSpan(0, SevenHours, 0, 0);
        public override int LocalCacheAbsoluteExpirationMinutes => 15;
        public override string CacheKey => string.Format(KeyFormat);
    }
}