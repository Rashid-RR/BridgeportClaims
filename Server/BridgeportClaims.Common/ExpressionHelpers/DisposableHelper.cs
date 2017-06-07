    using System;

namespace BridgeportClaims.Common.ExpressionHelpers
{
    public static class DisposableHelper
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