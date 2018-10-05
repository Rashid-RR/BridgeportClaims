using System;
using BridgeportClaims.RedisCache.Connection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StackExchange.Redis;
using cs = BridgeportClaims.Common.Config.ConfigService;
using s = BridgeportClaims.Common.Constants.StringConstants;

namespace BridgeportClaims.Tests.RedisCache
{
    [TestClass]
    public class RedisCacheTests
    {
        private const string Key = "myKey";
        private const string Value = "myValue";

        [TestMethod]
        public void CanSetAndGetFromRedisCache()
        {
            // Arrange.
            var redisCache = ConnectionService.Connection.GetDatabase();

            // Act.
            redisCache.StringSet(Key, Value);
            RedisValue data = redisCache.StringGet(Key);

            // Assert.
            Assert.IsTrue(data.HasValue);
            Assert.AreEqual(data, Value);
        }

        private static Lazy<ConnectionMultiplexer> CreateMultiplexer()
        {
            var connectionString = cs.GetAppSetting(s.RedisCacheConnection);
            return new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(connectionString));
        }
    }
}