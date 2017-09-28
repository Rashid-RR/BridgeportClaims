using System;
using System.Threading.Tasks;

namespace BridgeportClaims.Common.Caching
{
    public interface IMemoryCacher
    {
        string GetPaymentPostingCacheKey(string userId);
        void AddItem(string key, object value);
        void UpdateItem(string key, object value);
        object GetItem(string key);
        object GetItem(string key, bool remove);
        Task<T> AddOrGetExisting<T>(string key, Func<Task<T>> valueFactory);
        T AddOrGetExisting<T>(string key, Func<T> valueFactory);
        void DeleteIfExists(string key);
        bool Contains(string key);
        void DeleteAll();
    }
}