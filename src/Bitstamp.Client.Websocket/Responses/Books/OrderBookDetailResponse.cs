using System;
using System.Linq;
using System.Reactive.Subjects;
using Bitstamp.Client.Websocket.Communicator;
using Bitstamp.Client.Websocket.Json;
using Bitstamp.Client.Websocket.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Websocket.Client;

namespace Bitstamp.Client.Websocket.Responses.Books
{
    /// <summary>
    /// Full order book response L3 - first 100 orders on both sides
    /// </summary>
    public class OrderBookDetailResponse : ResponseBase
    {
        /// <summary>
        /// Order book detail event type
        /// </summary>
        public override MessageType Event => MessageType.OrderBookDetail;

        /// <summary>
        /// Order book detail data
        /// </summary>
        public OrderBookDetail Data { get; set; }

        internal static bool TryHandle(JObject response, ISubject<OrderBookDetailResponse> subject)
        {
            var channelName = response?["channel"];
            if (channelName == null || !channelName.Value<string>().StartsWith("detail_order_book"))
                return false;

            var parsed = response.ToObject<OrderBookDetailResponse>(BitstampJsonSerializer.Serializer);

            if (parsed != null)
            {
                parsed.Symbol = channelName.Value<string>().Split('_').LastOrDefault();
                subject.OnNext(parsed);
            }

            return true;
        }

        /// <summary>
        /// Stream snapshot manually via communicator
        /// </summary>
        public static void StreamFakeSnapshot(OrderBookSnapshotResponse snapshot, IBitstampCommunicator communicator)
        {
            var symbolSafe = (snapshot?.Symbol ?? string.Empty).ToLower().Replace("-", "").Replace("_", "");
            var response = snapshot;
            if (response != null)
            {
                response.Symbol = $"{symbolSafe}";

                var serialized = JsonConvert.SerializeObject(response, BitstampJsonSerializer.Settings);
                communicator.StreamFakeMessage(ResponseMessage.TextMessage(serialized));
            }
        }
    }
}