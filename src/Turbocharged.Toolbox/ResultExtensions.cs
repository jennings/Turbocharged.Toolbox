using System;
using System.Linq;

namespace Turbocharged.Toolbox
{
    public static class ResultExtensions
    {
        static class Lambdas<T>
        {
            public static Func<T, T> Identity { get; } = val => val;
        }

        public static T UnwrapOrThrow<T, TError>(this Result<T, TError> result)
            where TError : Exception
        {
            return result.UnwrapOrThrow(Lambdas<TError>.Identity);
        }

        public static Result<TOut, TError> Select<T, TError, TOut>(this Result<T, TError> result, Func<T, TOut> selector)
        {
            if (result.TryGetValue(out T value))
            {
                return selector(value);
            }

            // Must be an error
            result.TryGetError(out TError error);
            return error;
        }
    }
}
