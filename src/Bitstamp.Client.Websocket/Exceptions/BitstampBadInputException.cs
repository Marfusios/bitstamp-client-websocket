using System;

namespace Bitstamp.Client.Websocket.Exceptions
{
    /// <summary>
    /// Exception that indicates bad user input
    /// </summary>
    public class BitstampBadInputException : BitstampException
    {
        /// <inheritdoc />
        public BitstampBadInputException()
        {
        }

        /// <inheritdoc />
        public BitstampBadInputException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        public BitstampBadInputException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}