namespace BridgeportClaims.RedisCache.Redis
{
    public interface IRedisResult<T> 
    {
        bool Success { get; set; }
        T ReturnResult { get; set; }
    }
}