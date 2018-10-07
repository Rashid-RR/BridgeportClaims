namespace BridgeportClaims.RedisCache.Redis
{
    public class RedisResult<T> : IRedisResult<T>
    {
        public bool Success { get; set; }
        public T ReturnResult { get; set; }
    }
}