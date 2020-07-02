using System;

namespace Bitstamp.Client.Websocket.Exceptions
{
    /// <summary>
    /// Base Bitstamp exception
    /// </summary>
    public class BitstampException : Exception
    {
        /// <inheritdoc />
        public BitstampException()
        {
        }

        /// <inheritdoc />
        public BitstampException(string message)
            : base(message)
        {
        }

        /// <inheritdoc />
        public BitstampException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}