using System;
using System.Collections.Specialized;
using BridgeportClaims.RedisCache.Environment;
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

        public RedisDomain(Lazy<IRedisEnvironment> environment)
        {
            _environment = environment;
            _redisSettings = _environment.Value.RedisSettings.Value;
            _useRedis = cs.UseRedis;
            _serializer = new Lazy<ProtobufSerializer>();
        }
    }
}