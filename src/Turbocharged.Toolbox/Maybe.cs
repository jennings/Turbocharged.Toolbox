using System;

namespace Turbocharged.Toolbox
{
    /// <summary>
    /// Maybe represents an optional type, similar to F#'s Option type or
    /// Haskell's Maybe monad. If a null value is provided to the constructor,
    /// this will become a null Maybe value.
    /// </summary>
    /// <remarks>
    /// This class is intended to eliminate the NullReferenceException. Since
    /// this is a struct, this value can never be null. All operations on this
    /// type are safe and cannot throw.
    /// This type is named "Maybe" so it does not clash with F#'s Option type.
    /// </remarks>
    public struct Maybe<T>
    {
        readonly bool _hasValue;
        readonly T _value;

        /// <summary>
        /// Creates a Maybe value that contains a value.
        /// </summary>
        public Maybe(T value)
        {
            _value = value;
            _hasValue = true;
        }

        public static implicit operator Maybe<T>(T value)
        {
            return new Maybe<T>(value);
        }

        public bool HasValue
        {
            get
            {
                return _hasValue;
            }
        }

        public bool IsNull
        {
            get { return !HasValue; }
        }

        public bool TryGetValue(out T value)
        {
            value = _value;
            return _hasValue;
        }

        public Maybe<TResult> Match<TResult>(Func<T, TResult> onValue)
        {
            if (HasValue)
            {
                return new Maybe<TResult>(onValue(_value));
            }
            return new Maybe<TResult>();
        }

        public void Match(Action<T> onValue, Action onNull)
        {
            if (HasValue)
            {
                onValue(_value);
            }
            else
            {
                onNull();
            }
        }

        public T Unwrap(T defaultValue)
        {
            if (HasValue)
            {
                return _value;
            }
            return defaultValue;
        }

        public T Unwrap(Func<T> defaultValueFactory)
        {
            if (HasValue)
            {
                return _value;
            }
            return defaultValueFactory();
        }
    }
}
