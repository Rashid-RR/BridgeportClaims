using System;
using System.Threading.Tasks;
using BridgeportClaims.Common.Protobuf;
using BridgeportClaims.RedisCache.Connection;
using BridgeportClaims.RedisCache.Keys;
using BridgeportClaims.RedisCache.Redis;
using StackExchange.Redis;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.RedisCache.Domain
{
    public class RedisDomain : IRedisDomain
    {
        private readonly bool _useRedis;
        
        public RedisDomain()
        {
            _useRedis = cs.UseRedis;
        }
        
        private string DecorateKey(string cacheKey)
        {
            if (true) // TODO: Figure out if this is Prod.
            {
                return cacheKey;
            }
            return cacheKey + System.Environment.MachineName;
        }

        public async Task<IRedisResult<T>> GetAsync<T>(ICacheKey key)
            where T : class
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
                var redisCache = ConnectionService.Connection.GetDatabase();
                var data = await redisCache.StringGetAsync(DecorateKey(key.CacheKey),
                    CommandFlags.PreferSlave).ConfigureAwait(false);

                result.ReturnResult = (!data.IsNull)
                    ? ProtobufService.ProtoDeserialize<T>(data)
                    : default(T);
                result.Success = !data.IsNull;

                // (await _serializer.Value.DeserializeAsync<T>(data).ConfigureAwait(false))


                return result;
            }
            catch (Exception ex)
            {
                result.ReturnResult = default(T);
                result.Success = false;
                return result;
            }
        }

        public async Task<bool> AddAsync<T>(ICacheKey key, T value, TimeSpan expirationTime)
            where T : class
        {
            if (!_useRedis)
            {
                return false;
            }
            try
            {
                var valueToCache = ProtobufService.ProtoSerialize(value);
                var redisCache = ConnectionService.Connection.GetDatabase();
                await redisCache.StringSetAsync(DecorateKey(key.CacheKey)
                    , valueToCache,
                    expirationTime, flags: CommandFlags.DemandMaster
                ).ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                return false;
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
                var redisCache = ConnectionService.Connection.GetDatabase();
                await redisCache.KeyDeleteAsync(DecorateKey(key.CacheKey), CommandFlags.DemandMaster).ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #region Serialization Helper



        #endregion

        #region Hash Operations
        /*
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
        */
        #endregion
    }
}