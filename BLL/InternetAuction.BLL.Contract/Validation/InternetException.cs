using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace InternetAuction.BLL.Contract.Validation
{
    /// <summary>
    /// The internet exception.
    /// </summary>
    public class InternetException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InternetException"/> class.
        /// </summary>
        public InternetException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InternetException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public InternetException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InternetException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public InternetException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InternetException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}