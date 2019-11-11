using System;
using System.Reflection;
using System.Threading.Tasks;
using BridgeportClaims.Common.Protobuf;
using BridgeportClaims.RedisCache.Connection;
using BridgeportClaims.RedisCache.Keys.Abstractions;
using BridgeportClaims.RedisCache.Redis;
using NLog;
using StackExchange.Redis;
using cs = BridgeportClaims.Common.Config.ConfigService;
using s = BridgeportClaims.Common.Constants.StringConstants;

namespace BridgeportClaims.RedisCache.Domain
{
    public class RedisDomain : IRedisDomain
    {
        private readonly bool _useRedis;
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);

        public RedisDomain()
        {
            _useRedis = cs.UseRedis;
        }
        
        private static string DecorateKey(string cacheKey)
        {
            if (cs.IsProduction)
            {
                return cacheKey;
            }
            if (cs.AppIsInDebugMode)
            {
                var method = MethodBase.GetCurrentMethod().Name;
                var now = DateTime.Now.ToString(s.TimeFormat);
                Logger.Value.Info($"We are outside of Production. Writing info message in the {method} method on {now}.");
            }
            return cacheKey + Environment.MachineName;
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
                var redisCache = RedisCacheConnectionHelper.Connection.GetDatabase();
                RedisValue data = await redisCache.StringGetAsync(DecorateKey(key.CacheKey),
                    CommandFlags.PreferSlave).ConfigureAwait(false);

                result.ReturnResult = !data.IsNull
                    ? ProtobufService.ProtoDeserialize<T>(data)
                    : default(T);
                result.Success = !data.IsNull;
                return result;
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
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
                var redisDb = RedisCacheConnectionHelper.Connection.GetDatabase();
                var valueToCache = ProtobufService.ProtoSerialize(value);
                await redisDb.StringSetAsync(DecorateKey(key.CacheKey)
                    , valueToCache, expirationTime, flags: CommandFlags.None
                ).ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
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
                var redisCache = RedisCacheConnectionHelper.Connection.GetDatabase();
                await redisCache.KeyDeleteAsync(DecorateKey(key.CacheKey), 
                    CommandFlags.DemandMaster).ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
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
                var db = RedisCacheConnectionHelper.Connection.GetDatabase();
                await db.KeyExpireAsync(DecorateKey(key.CacheKey), expirationTime, 
                    CommandFlags.DemandMaster).ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return false;
            }
        }

        public async Task<bool> KeyExists(ICacheKey key)
        {
            if (!_useRedis)
            {
                return false;
            }
            try
            {
                var db = RedisCacheConnectionHelper.Connection.GetDatabase();
                var retVal = await db.KeyExistsAsync(DecorateKey(key.CacheKey), 
                    CommandFlags.PreferSlave);
                return retVal;
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return false;
            }
        }
    }
}