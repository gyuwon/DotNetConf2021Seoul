using System;

namespace Sample
{
    public class Maybe<T>
    {
        private readonly bool hasValue;
        private readonly T value;

        private Maybe() => hasValue = false;

        internal Maybe(T value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            (hasValue, this.value) = (true, value);
        }

        public static Maybe<T> Nothing { get; } = new Maybe<T>();

        public Maybe<U> Map<U>(Func<T, U> map)
            => hasValue ? new Maybe<U>(map.Invoke(value)) : Maybe<U>.Nothing;

        public Maybe<U> Bind<U>(Func<T, Maybe<U>> map)
            => hasValue ? map.Invoke(value) : Maybe<U>.Nothing;
    }

    public static class Maybe
    {
        public static Maybe<T> Just<T>(T value)
            => new Maybe<T>(value);

        public static Maybe<T> Create<T>(T value) where T : class
            => value == null ? Maybe<T>.Nothing : Just(value);
    }
}
