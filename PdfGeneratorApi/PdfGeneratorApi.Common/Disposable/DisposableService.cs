using System;

namespace PdfGeneratorApi.Common.Disposable
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

        public static void Using<TDisposable>(Func<TDisposable> factory,
            Action<TDisposable> map) where TDisposable : IDisposable
        {
            using (var disposable = factory())
            {
                map(disposable);
            }
        }
    }
}