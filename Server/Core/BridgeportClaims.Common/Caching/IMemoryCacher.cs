using System;
using System.Threading.Tasks;

namespace BridgeportClaims.Common.Caching
{
    public interface IMemoryCacher
    {
        string GetPaymentPostingCacheKey(string userId);
        void UpdateItem(string key, object value);
        Task<T> AddOrGetExisting<T>(string key, Func<Task<T>> valueFactory);
        T AddOrGetExisting<T>(string key, Func<T> valueFactory);
        void Delete(string key);
        bool Contains(string key);
        void DeleteAll();
    }
}