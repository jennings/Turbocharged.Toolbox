using System;

namespace Turbocharged.Toolbox
{
    public struct Result<T, TError>
    {
        T _value;
        TError _error;
        bool _hasValue;
        bool _hasError;

        public Result(T value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            _value = value;
            _error = default(TError);
            _hasValue = true;
            _hasError = false;
        }

        public Result(TError error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            _value = default(T);
            _error = error;
            _hasValue = false;
            _hasError = true;
        }

        public static Result<T, TError> Value(T value)
        {
            return new Result<T, TError>(value);
        }

        public static Result<T, TError> Error(TError error)
        {
            return new Result<T, TError>(error);
        }

        public static implicit operator Result<T, TError>(T value)
        {
            return new Result<T, TError>(value);
        }

        public static implicit operator Result<T, TError>(TError error)
        {
            return new Result<T, TError>(error);
        }

        void ThrowIfUnitialized()
        {
            if (!_hasValue && !_hasError)
                throw new InvalidOperationException("Result has not been initialized.");
        }

        public bool HasValue
        {
            get
            {
                ThrowIfUnitialized();
                return _hasValue;
            }
        }

        public bool HasError
        {
            get
            {
                ThrowIfUnitialized();
                return _hasError;
            }
        }

        public bool TryGetValue(out T value)
        {
            ThrowIfUnitialized();
            if (HasValue)
            {
                value = _value;
                return true;
            }
            value = default(T);
            return false;
        }

        public bool TryGetError(out TError error)
        {
            ThrowIfUnitialized();
            if (HasError)
            {
                error = _error;
                return true;
            }
            error = default(TError);
            return false;
        }

        public T Unwrap(T onError)
        {
            ThrowIfUnitialized();
            if (HasValue)
            {
                return _value;
            }
            return onError;
        }

        public T Unwrap(Func<TError, T> onError)
        {
            ThrowIfUnitialized();
            if (HasValue)
            {
                return _value;
            }
            return onError(_error);
        }

        public T UnwrapOrThrow<TException>(Func<TError, TException> exceptionFactory)
            where TException : Exception
        {
            ThrowIfUnitialized();
            if (HasValue)
            {
                return _value;
            }
            throw exceptionFactory(_error);
        }
    }
}
