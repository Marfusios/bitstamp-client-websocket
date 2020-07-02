using System.Linq;
using System.Reactive.Subjects;
using Bitstamp.Client.Websocket.Json;
using Bitstamp.Client.Websocket.Messages;
using Newtonsoft.Json.Linq;

namespace Bitstamp.Client.Websocket.Responses.Books
{
    /// <summary>
    /// Diff order book response L2 - full order book
    /// </summary>
    public class OrderBookDiffResponse : ResponseBase
    {
        /// <summary>
        /// Order book diff event type
        /// </summary>
        public override MessageType Event => MessageType.OrderBookDiff;

        /// <summary>
        /// Order book diff data
        /// </summary>
        public OrderBookDiff Data { get; set; }

        internal static bool TryHandle(JObject response, ISubject<OrderBookDiffResponse> subject)
        {
            var channelName = response?["channel"];
            if (channelName == null || !channelName.Value<string>().StartsWith("diff_order_book"))
                return false;

            var parsed = response?.ToObject<OrderBookDiffResponse>(BitstampJsonSerializer.Serializer);

            if (parsed != null)
            {
                parsed.Symbol = channelName.Value<string>().Split('_').LastOrDefault();
                subject.OnNext(parsed);
            }

            return true;
        }
    }
}