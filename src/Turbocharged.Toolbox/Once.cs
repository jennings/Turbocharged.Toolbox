using System;

namespace Turbocharged.Toolbox
{
    /// <summary>
    /// An object that executes an action exactly once. This can be used to
    /// guard against initialization code executing multiple times.
    /// </summary>
    public sealed class Once
    {
        readonly object _lock = new object();
        bool _finished;
        Action _action;
        Exception _error;

        public Once(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            _action = action;
        }

        /// <summary>
        /// Executes the given action if this is the first time <c>Do</c> has
        /// been called. Future calls will return without invoking the action.
        /// If the action is executed and throws, this instance will be
        /// considered poisoned and will throw every time <c>Execute</c> is
        /// called.
        /// </summary>
        public void Execute()
        {
            // Check for doneness outside the lock
            if (_finished) return;
            if (_error != null) throw MakeException(_error);

            lock (_lock)
            {
                // Check again, in case two threads called this method
                // simultaneously.
                if (_finished) return;
                if (_error != null) throw MakeException(_error);

                try
                {
                    _action();
                    // If the action completes successfully, future invocations
                    // will return immediately.
                    _finished = true;
                    _action = null;
                }
                catch (Exception ex)
                {
                    // This and all future invocations should throw.
                    _error = ex;
                    _action = null;
                    throw MakeException(_error);
                }
            }
        }

        static PoisonException MakeException(Exception error)
        {
            return new PoisonException("Once instance has been poisoned.", error);
        }
    }
}
