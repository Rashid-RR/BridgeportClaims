using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BridgeportClaims.Common.Helpers
{
    public class AsyncLazy<T> : Lazy<Task<T>>
    {
        public AsyncLazy(Func<T> valueFactory) : base(() => Task.Factory.StartNew(valueFactory)) { }

        // ReSharper disable once ConvertClosureToMethodGroup
        public AsyncLazy(Func<Task<T>> taskFactory) : base(() => Task.Factory.StartNew(() => taskFactory()).Unwrap()) { }

        public TaskAwaiter<T> GetAwaiter() { return Value.GetAwaiter(); }
    }
}
