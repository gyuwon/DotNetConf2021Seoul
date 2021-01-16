using System;

namespace Sample
{
    public readonly struct Maybe<T>
    {
        private readonly bool hasValue;
        private readonly T value;

        internal Maybe(T value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            (hasValue, this.value) = (true, value);
        }

        public static Maybe<T> Nothing => new Maybe<T>();

        public Maybe<U> Map<U>(Func<T, U> map)
            => hasValue ? new Maybe<U>(map.Invoke(value)) : Maybe<U>.Nothing;

        public static Maybe<T> Join(Maybe<Maybe<T>> m)
            => m.hasValue && m.value.hasValue ? new Maybe<T>(m.value.value) : Nothing;

        public Maybe<U> Bind<U>(Func<T, Maybe<U>> map)
            => Maybe<U>.Join(Map(map));
    }

    public static class Maybe
    {
        public static Maybe<T> Just<T>(T value) where T : struct
            => new Maybe<T>(value);

        public static Maybe<T> Create<T>(T value) where T : class
            => value == null ? Maybe<T>.Nothing : new Maybe<T>(value);

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
