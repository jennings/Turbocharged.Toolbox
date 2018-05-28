using System;

namespace Turbocharged.Toolbox
{
    /// <summary>
    /// Indicates that an object is in an invalid state and cannot be used.
    /// </summary>
    public class PoisonException : InvalidOperationException
    {
        public PoisonException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
