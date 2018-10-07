using System;
using System.Threading.Tasks;

namespace BridgeportClaims.Common.Async
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AsyncLazy<T> : Lazy<Task<T>>
    {
        public AsyncLazy(Func<T> valueFactory) :
            base(() => Task.Factory.StartNew(valueFactory)) { }

        public AsyncLazy(Func<Task<T>> taskFactory) :
            base(() => Task.Factory.StartNew(taskFactory).Unwrap()) { }
    }
    // Usage
    /*
        private static AsyncLazy<string> _mData = new AsyncLazy<string>(async delegate
        {
            var client = new WebClient();
            return (await client.DownloadStringTaskAsync("http://www.microsoft.com")).ToUpper();
        });
    */
}