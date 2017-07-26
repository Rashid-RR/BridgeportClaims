using System;

namespace BridgeportClaims.Common.Caching
{
    public interface IMemoryCacher
    {
        object GetValue(string key);
        bool Add(string key, object value, DateTimeOffset absExpiration);
        void Delete(string key);
    }
}