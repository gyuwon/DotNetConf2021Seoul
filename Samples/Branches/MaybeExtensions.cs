using System;

namespace Sample
{
    public static class MaybeExtensions
    {
        public static void Do<T>(this Maybe<T> maybe, Action<T> forJust, Action forNothing)
        {
            bool just = false;

            maybe.Map(some =>
            {
                forJust.Invoke(some);
                just = true;
                return true;
            });

            if (just == false)
            {
                forNothing.Invoke();
            }
        }

        public static void DoIfJust<T>(this Maybe<T> maybe, Action<T> forJust)
            => maybe.Do(forJust, () => { });

        public static Maybe<TResult> SelectMany<TSource, TCollection, TResult>(
            this Maybe<TSource> source,
            Func<TSource, Maybe<TCollection>> collectionSelector,
            Func<TSource, TCollection, TResult> resultSelector)
        {
            return source.Bind(x => collectionSelector.Invoke(x).Map(y => resultSelector.Invoke(x, y)));
        }
    }
}
