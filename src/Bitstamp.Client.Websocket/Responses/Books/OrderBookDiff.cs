using System;
using Bitstamp.Client.Websocket.Json;
using Newtonsoft.Json;

namespace Bitstamp.Client.Websocket.Responses.Books
{
    public class OrderBookDiff
    {
        /// <summary>
        /// Timestamp - datetime only seconds
        /// </summary>
        [JsonConverter(typeof(UnixDateTimeSecondsConverter))]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Microtimestamp - datetime with milliseconds
        /// </summary>
        [JsonConverter(typeof(UnixDateTimeMillisecondsConverter))]
        public DateTime Microtimestamp { get; set; }

        /// <summary>
        /// Order book bid levels
        /// </summary>
        [JsonConverter(typeof(OrderBookLevelConverter), OrderBookSide.Bid)]
        public BookLevel[] Bids { get; set; }

        /// <summary>
        /// Order book ask levels
        /// </summary>
        [JsonConverter(typeof(OrderBookLevelConverter), OrderBookSide.Ask)]
        public BookLevel[] Asks { get; set; }
    }
}