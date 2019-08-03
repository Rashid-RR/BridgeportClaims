using System;
using System.Threading;
using System.Threading.Tasks;
using BridgeportClaims.RedisCache.Connection;
using BridgeportClaims.RedisCache.Domain;
using BridgeportClaims.RedisCache.Keys.Abstractions;
using BridgeportClaims.Tests.Protobuf.Models;
using BridgeportClaims.Tests.RedisCache.Keys;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StackExchange.Redis;

namespace BridgeportClaims.Tests.RedisCache
{
    [TestClass]
    public class RedisCacheTests
    {
        private const string Key = "myKey";
        private const string Value = "myValue";
        private readonly Lazy<IRedisDomain> _redisDomain = new Lazy<IRedisDomain>(() => new RedisDomain());

        [TestMethod]
        public void CanSetAndGetFromRedisCache()
        {
            // Arrange.
            var redisCache = CacheConnectionHelper.Connection.GetDatabase();

            // Act.
            redisCache.StringSet(Key, Value);
            RedisValue data = redisCache.StringGet(Key);

            // Assert.
            Assert.IsTrue(data.HasValue);
            Assert.AreEqual(data, Value);
        }

        [TestMethod]
        [Ignore]
        public async Task CanSetAndGetFromRedisObjects()
        {
            // Arrange.
            var cacheKey = new PersonCacheKey();

            // Act.
            var result = await _redisDomain.Value.GetAsync<Person>(cacheKey).ConfigureAwait(false);
            var kevinDurant = result.ReturnResult;
            if (!result.Success || null == kevinDurant)
            {
                kevinDurant = PersonBuilder.BuildPerson();
                if (null != kevinDurant)
                {
                    await _redisDomain.Value.AddAsync(cacheKey, kevinDurant, cacheKey.RedisExpirationTimespan)
                        .ConfigureAwait(false);
                }
            }

            // Assert.
            var personCacheKeyExists = await _redisDomain.Value.KeyExists(cacheKey).ConfigureAwait(false);
            Assert.IsTrue(personCacheKeyExists);
            Assert.IsNotNull(kevinDurant);
            var copyOfKd = PersonBuilder.BuildPerson();
            Assert.AreEqual(kevinDurant.FirstName, copyOfKd.FirstName);
            Assert.AreEqual(kevinDurant.LastName, copyOfKd.LastName);
            Assert.AreEqual(kevinDurant.JoinDate, copyOfKd.JoinDate);
            // Can't compare non-protobuf members.

            // Act. Assert.
            var newResult = await _redisDomain.Value.GetAsync<Person>(cacheKey).ConfigureAwait(false);
            var newKd = newResult.ReturnResult;
            Assert.IsTrue(newResult.Success);
            Assert.IsNotNull(newKd);

            // Wait ten seconds for the Cache to Expire.
            Thread.Sleep(new TimeSpan(0, 0, 0, 10));

            // Act. Assert.
            var expiredResult = await _redisDomain.Value.GetAsync<Person>(cacheKey).ConfigureAwait(false);
            var nothing = expiredResult.ReturnResult;
            Assert.IsFalse(expiredResult.Success);
            Assert.IsNull(nothing);

            ICacheKey fakeCacheKey = new FakeCacheKey();
            var cacheKeyDoesNotExist = await _redisDomain.Value.KeyExists(fakeCacheKey)
                .ConfigureAwait(false);
            Assert.IsFalse(cacheKeyDoesNotExist);
        }

        private string DecorateFakeKey(string cacheKey)
        {
            return cacheKey + "Fake";
        }
    }
}