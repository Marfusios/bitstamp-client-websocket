using System.Linq;
using System.Reactive.Subjects;
using Bitstamp.Client.Websocket.Json;
using Bitstamp.Client.Websocket.Messages;
using Newtonsoft.Json.Linq;

namespace Bitstamp.Client.Websocket.Responses.Books
{
    /// <summary>
    /// Full order book response L2 - first 100 levels on both sides
    /// </summary>
    public class OrderBookResponse : ResponseBase
    {
        /// <summary>
        /// Order book event type
        /// </summary>
        public override MessageType Event => MessageType.OrderBook;

        /// <summary>
        /// Order book data
        /// </summary>
        public OrderBook Data { get; set; }

        internal static bool TryHandle(JObject response, ISubject<OrderBookResponse> subject)
        {
            var channelName = response?["channel"];
            if (channelName == null || !channelName.Value<string>().StartsWith("order_book")) 
                return false;

            var parsed = response?.ToObject<OrderBookResponse>(BitstampJsonSerializer.Serializer);
            if (parsed != null)
            {
                parsed.Symbol = channelName.Value<string>().Split('_').LastOrDefault();
                subject.OnNext(parsed);
            }

            return true;
        }
    }
}