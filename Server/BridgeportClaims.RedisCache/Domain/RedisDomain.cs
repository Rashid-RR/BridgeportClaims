using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using BridgeportClaims.RedisCache.Environment;
using BridgeportClaims.RedisCache.Keys;
using BridgeportClaims.RedisCache.Redis;
using ProtoBuf;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Protobuf;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.RedisCache.Domain
{
    public class RedisDomain
    {
        private readonly bool _useRedis;
        private readonly NameValueCollection _redisSettings;
        private readonly Lazy<IRedisEnvironment> _environment;
        private readonly Lazy<ProtobufSerializer> _serializer;

        private readonly Lazy<StackExchangeRedisCacheClient> _cacheClient =
            new Lazy<StackExchangeRedisCacheClient>();

        public RedisDomain(Lazy<IRedisEnvironment> environment)
        {
            _environment = environment;
            _redisSettings = _environment.Value.RedisSettings.Value;
            _useRedis = cs.UseRedis;
            _serializer = new Lazy<ProtobufSerializer>();
        }

        private string DecorateKey(string cacheKey)
        {
            if (_environment.Value.IsProduction)
            {
                return cacheKey;
            }

            return cacheKey + System.Environment.MachineName;
        }

        #region Serialization Helper

        public Task<byte[]> SerializeObjectAsync<T>(T obj)
        {
            return _serializer.Value.SerializeAsync(obj);
        }

        public Task<T> DeserializeObjectAsync<T>(byte[] objData)
        {
            return _serializer.Value.DeserializeAsync<T>(objData);
        }

        #endregion

        #region Hash Operations

        public async Task<bool> AddHashEntryAsync<T>(ICacheKey key, KeyValuePair<ICacheKey, T> entry)
        {
            if (!_useRedis)
            {
                return false;
            }

            try
            {
                var db = _cacheClient.Value.Database;
                await db.HashSetAsync(
                    DecorateKey(key.CacheKey),
                    new[]
                    {
                        new HashEntry(
                            entry.Key.CacheKey,
                            (await _cacheClient.Value.Serializer.SerializeAsync(entry.Value).ConfigureAwait(false)))
                    },
                    CommandFlags.DemandMaster
                ).ConfigureAwait(false);
                return true;
            }
            catch (Exception)// ex)
            {
                return false;
            }
        }

        public async Task<bool> AddHashEntriesAsync<T>(ICacheKey key, Dictionary<ICacheKey, T> entries)
        {
            if (!_useRedis)
            {
                return false;
            }

            try
            {
                var db = _cacheClient.Value.Database;
                HashEntry[] hashEntries = new HashEntry[entries.Count];
                int index = 0;

                foreach (var entry in entries)
                {
                    var keyVal = (await _cacheClient.Value.Serializer.SerializeAsync(entry.Value)
                        .ConfigureAwait(false));
                    hashEntries[index++] = new HashEntry(entry.Key.CacheKey, keyVal);
                }

                await db.HashSetAsync(DecorateKey(key.CacheKey), hashEntries, CommandFlags.DemandMaster)
                    .ConfigureAwait(false);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IRedisResult<T>> GetHashEntryAsync<T>(ICacheKey key, ICacheKey name)
        {
            IRedisResult<T> result = new RedisResult<T>();
            if (!_useRedis)
            {
                result.ReturnResult = default(T);
                result.Success = false;
                return result;
            }

            try
            {
                var db = _cacheClient.Value.Database;
                var data = await db.HashGetAsync(DecorateKey(key.CacheKey), name.CacheKey, CommandFlags.PreferSlave)
                    .ConfigureAwait(false);
                result.ReturnResult = (!data.IsNull)
                    ? (await _cacheClient.Value.Serializer.DeserializeAsync<T>(data).ConfigureAwait(false))
                    : default(T);
                result.Success = !data.IsNull;

                return result;
            }
            catch (Exception)
            {
                result.ReturnResult = default(T);
                result.Success = false;
                return result;
            }
        }

        public async Task<Dictionary<string, IRedisResult<T>>> GetHashEntryAsync<T>(ICacheKey key)
        {
            Dictionary<string, IRedisResult<T>> resultCollection = new Dictionary<string, IRedisResult<T>>();
            if (!_useRedis)
            {
                return resultCollection;
            }

            try
            {
                var db = _cacheClient.Value.Database;
                var x = (await db.HashGetAllAsync(DecorateKey(key.CacheKey), CommandFlags.PreferSlave)
                    .ConfigureAwait(false));

                foreach (HashEntry entry in x)
                {
                    if (resultCollection.ContainsKey(entry.Name))
                    {
                        continue;
                    }

                    IRedisResult<T> result = new RedisResult<T>
                    {
                        ReturnResult = (!entry.Value.IsNull
                            ? (await _cacheClient.Value.Serializer.DeserializeAsync<T>(entry.Value)
                                .ConfigureAwait(false))
                            : default(T)),
                        Success = !entry.Value.IsNull
                    };
                    resultCollection[entry.Name] = result;
                }

                return resultCollection;
            }
            catch (Exception)
            {
                resultCollection = new Dictionary<string, IRedisResult<T>>();
                return resultCollection;
            }
        }

        public async Task RemoveHashEntryAsync(ICacheKey key, ICacheKey name)
        {
            if (!_useRedis)
            {
                return;
            }

            try
            {
                var db = _cacheClient.Value.Database;
                await db.HashDeleteAsync(DecorateKey(key.CacheKey), name.CacheKey, CommandFlags.DemandMaster)
                    .ConfigureAwait(false);
            }
            catch
            {
                // ignored.
            }
        }

        public async Task<bool> RemoveHashAsync(ICacheKey key)
        {
            if (!_useRedis)
            {
                return false;
            }

            try
            {
                var db = _cacheClient.Value.Database;
                var entries = await db.HashGetAllAsync(DecorateKey(key.CacheKey), CommandFlags.DemandMaster)
                    .ConfigureAwait(false);
                var batch = db.CreateBatch();
                foreach (var entry in entries)
                {
                    await batch.HashDeleteAsync(DecorateKey(key.CacheKey), entry.Name, CommandFlags.DemandMaster)
                        .ConfigureAwait(false);
                }

                batch.Execute();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region Key Operations

        public async Task<bool> AddAsync<T>(ICacheKey key, T value, TimeSpan expirationTime)
        {
            if (!_useRedis)
            {
                return false;
            }

            try
            {
                var db = _cacheClient.Value.Database;
                await db.StringSetAsync(
                    DecorateKey(key.CacheKey),
                    (await _cacheClient.Value.Serializer.SerializeAsync(value).ConfigureAwait(false)),
                    expirationTime,
                    flags: CommandFlags.DemandMaster
                ).ConfigureAwait(false);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IRedisResult<T>> GetAsync<T>(ICacheKey key)
        {
            IRedisResult<T> result = new RedisResult<T>();
            if (!_useRedis)
            {
                result.ReturnResult = default(T);
                result.Success = false;
                return result;
            }

            try
            {
                var db = _cacheClient.Value.Database;
                var data = await db.StringGetAsync(
                    DecorateKey(key.CacheKey),
                    CommandFlags.PreferSlave
                ).ConfigureAwait(false);

                result.ReturnResult = (!data.IsNull)
                    ? (await _cacheClient.Value.Serializer.DeserializeAsync<T>(data).ConfigureAwait(false))
                    : default(T);
                result.Success = !data.IsNull;

                return result;
            }
            catch (Exception)
            {
                result.ReturnResult = default(T);
                result.Success = false;
                return result;
            }
        }

        public async Task<Dictionary<ICacheKey, IRedisResult<T>>> GetMultipleAsync<T>(IEnumerable<ICacheKey> keys)
        {
            var result = new Dictionary<ICacheKey, IRedisResult<T>>();
            if (keys == null || !keys.Any())
            {
                return result;
            }
            Dictionary<RedisKey, ICacheKey> decoratedKeyList =
                keys.ToDictionary(y => (RedisKey) DecorateKey(y.CacheKey), x => x);
            RedisKey[] decoratedKeyArray = decoratedKeyList.Keys.ToArray();
            foreach (var item in decoratedKeyList)
            {
                result[item.Value] = new RedisResult<T>
                {
                    Success = false,
                    ReturnResult = default(T)
                };
            }
            if (!_useRedis)
            {
                return result;
            }
            try
            {
                var db = _cacheClient.Value.Database;
                var dataArray = await db.StringGetAsync(decoratedKeyArray, CommandFlags.PreferSlave)
                    .ConfigureAwait(false);
                for (var dataIterator = 0; dataIterator < dataArray.Length; dataIterator++)
                {
                    var data = dataArray[dataIterator];
                    result[decoratedKeyList[decoratedKeyArray[dataIterator]]].ReturnResult = (!data.IsNull)
                        ? (await _cacheClient.Value.Serializer.DeserializeAsync<T>(data).ConfigureAwait(false))
                        : default(T);
                    result[decoratedKeyList[decoratedKeyArray[dataIterator]]].Success = !data.IsNull;
                }

                return result;
            }
            catch (Exception)
            {
                foreach (var item in result)
                {
                    item.Value.Success = false;
                    item.Value.ReturnResult = default(T);
                }

                return result;

            }
        }

        public async Task<bool> RemoveAsync(ICacheKey key)
        {
            if (!_useRedis)
            {
                return false;
            }

            try
            {
                var db = _cacheClient.Value.Database;
                await db.KeyDeleteAsync(DecorateKey(key.CacheKey), CommandFlags.DemandMaster).ConfigureAwait(false);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> SetKeyExpirationAsync(ICacheKey key, TimeSpan expirationTime)
        {
            if (!_useRedis)
            {
                return false;
            }
            try
            {
                var db = _cacheClient.Value.Database;
                await db.KeyExpireAsync(DecorateKey(key.CacheKey), expirationTime, CommandFlags.DemandMaster)
                    .ConfigureAwait(false);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}