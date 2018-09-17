using System;

namespace BridgeportClaims.Maintenance.Business.Disposable
{
    internal static class DisposableService
    {
        internal static TResult Using<TDisposable, TResult>(Func<TDisposable> factory,
            Func<TDisposable, TResult> map) where TDisposable : IDisposable
        {
            using (var disposable = factory())
            {
                return map(disposable);
            }
        }

        internal static void Using<TDisposable>(Func<TDisposable> factory,
            Action<TDisposable> map) where TDisposable : IDisposable
        {
            using (var disposable = factory())
            {
                map(disposable);
            }
        }
    }
}