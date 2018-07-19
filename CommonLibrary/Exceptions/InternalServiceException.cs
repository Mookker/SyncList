using System;

namespace SyncList.CommonLibrary.Exceptions
{
    /// <summary>
    /// Custom exception for internal services.
    /// </summary>
    /// <seealso cref="System.Exception" />
    class InternalServiceException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InternalServiceException"/> class.
        /// </summary>
        /// <param name="inner">The inner exception.</param>
        public InternalServiceException(Exception inner) : base("Internal Service Exception.", inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalServiceException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        public InternalServiceException(string message, Exception inner): base(message, inner)
        {
        }
    }
}