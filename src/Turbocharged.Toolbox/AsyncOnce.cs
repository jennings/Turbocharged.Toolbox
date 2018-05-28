using System;
using System.Threading;
using System.Threading.Tasks;

namespace Turbocharged.Toolbox
{
    /// <summary>
    /// An object that executes an async function exactly once. This can be
    /// used to guard against initialization code executing multiple times.
    /// </summary>
    public sealed class AsyncOnce
    {
        SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        bool _finished;
        Func<Task> _func;
        Exception _error;

        public AsyncOnce(Func<Task> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            _func = func;
        }

        /// <summary>
        /// Executes the given action if this is the first time <c>Do</c> has
        /// been called. Future calls will return without invoking the action.
        /// If the action is executed and throws, this instance will be
        /// considered poisoned and will throw every time <c>Execute</c> is
        /// called.
        /// </summary>
        public async Task ExecuteAsync()
        {
            // Check for doneness outside the lock
            if (_finished) return;
            if (_error != null) throw MakeException(_error);

            _lock.Wait();
            try
            {
                // Check again, in case two threads called this method
                // simultaneously.
                if (_finished) return;
                if (_error != null) throw MakeException(_error);

                try
                {
                    await _func();
                    // If the action completes successfully, future invocations
                    // will return immediately.
                    _finished = true;
                    _func = null;
                }
                catch (Exception ex)
                {
                    // This and all future invocations should throw.
                    _error = ex;
                    _func = null;
                    throw MakeException(_error);
                }
            }
            finally
            {
                _lock.Release();
            }
        }

        static PoisonException MakeException(Exception error)
        {
            return new PoisonException("Once instance has been poisoned.", error);
        }
    }
}
