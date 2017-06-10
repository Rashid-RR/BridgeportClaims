    using System;

namespace BridgeportClaims.Common.ExpressionManagers
{
    public static class DisposableManager
    {
        public static TResult Using<TDisposable, TResult>(
            Func<TDisposable> factory,
            Func<TDisposable, TResult> map)
            where TDisposable : IDisposable
        {
            using (var disposable = factory())
            {
                return map(disposable);
            }
        }
    }
}