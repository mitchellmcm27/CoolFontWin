using System;
using System.Linq;
using System.Reactive.Linq;


namespace CFW.Business.Extensions
{
    public static class ObservableExtensions
    {
        public static IObservable<T> StepInterval<T>(this IObservable<T> source, TimeSpan minDelay)
        {
            return source.Select(x =>
                Observable.Empty<T>()
                    .Delay(minDelay)
                    .StartWith(x)
            ).Concat();
        }
    }
}
