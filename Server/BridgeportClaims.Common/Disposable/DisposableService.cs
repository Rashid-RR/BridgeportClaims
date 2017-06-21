using System;

namespace BridgeportClaims.Common.Disposable
{
    public static class DisposableService
    {
        public static TResult Using<TDisposable, TResult>(Func<TDisposable> factory,
            Func<TDisposable, TResult> map) where TDisposable : IDisposable
        {
            using (var disposable = factory())
            {
                return map(disposable);
            }
        }
    }
}