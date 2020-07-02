using System;
using System.Linq;
using System.Reactive.Subjects;
using Bitstamp.Client.Websocket.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bitstamp.Client.Websocket.Responses
{
    /// <summary>
    /// Trade (ticker) response
    /// </summary>
    public class Ticker : ResponseBase
    {
        /// <summary>
        /// Trade/ticker data
        /// </summary>
        public TickerData Data { get; set; }

        internal static bool TryHandle(JObject response, ISubject<Ticker> subject)
        {
            var eventName = response?["event"];
            if (eventName == null || eventName.Value<string>() != "trade")
                return false;

            var parsed = response.ToObject<Ticker>(BitstampJsonSerializer.Serializer);

            var channelName = response["channel"];
            if (parsed != null && channelName != null)
            {
                parsed.Symbol = channelName.Value<string>().Split('_').LastOrDefault();
                subject.OnNext(parsed);
            }
            
            return true;
        }
    }

    /// <summary>
    /// Trade (ticker) data
    /// </summary>
    public class TickerData
    {
        /// <summary>
        /// Trade unique id
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// Microtimestamp - datetime with milliseconds
        /// </summary>
        [JsonProperty("microtimestamp")]
        [JsonConverter(typeof(UnixDateTimeMillisecondsConverter))]
        public DateTime Microtimestamp { get; set; }

        /// <summary>
        /// Timestamp - datetime only seconds
        /// </summary>
        [JsonProperty("timestamp")]
        [JsonConverter(typeof(UnixDateTimeSecondsConverter))]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Trade amount - size
        /// </summary>
        [JsonProperty("amount")]
        public double Amount { get; set; }

        /// <summary>
        /// Trade amount represented in string format
        /// </summary>
        [JsonProperty("amount_str")]
        public string AmountStr { get; set; }

        /// <summary>
        /// Trade buy order ID
        /// </summary>
        [JsonProperty("buy_order_id")]
        public long BuyOrderId { get; set; }

        /// <summary>
        /// Trade sell order ID.
        /// </summary>
        [JsonProperty("sell_order_id")]
        public long SellOrderId { get; set; }

        /// <summary>
        /// Trade price
        /// </summary>
        [JsonProperty("price")]
        public double Price { get; set; }

        /// <summary>
        /// Trade price represented in string format
        /// </summary>
        [JsonProperty("price_str")]
        public string PriceStr { get; set; }

        /// <summary>
        /// Trade type (0 - buy; 1 - sell)
        /// </summary>
        [JsonProperty("type")]
        public long Type { get; set; }

        /// <summary>
        /// Trade side
        /// </summary>
        [JsonIgnore]
        public TradeSide Side => Type == 0 ? TradeSide.Buy :
            Type == 1 ? TradeSide.Sell :
            TradeSide.Undefined;
    }
}