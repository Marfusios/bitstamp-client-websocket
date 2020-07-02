using Newtonsoft.Json;

namespace Bitstamp.Client.Websocket.Responses.Orders
{
    public class Order
    {
        public string MicroTimeStamp { get; set; }

        public double Amount { get; set; }

        [JsonProperty("order_type")] public long OrderType { get; set; }

        [JsonProperty("amount_str")] public string AmountStr { get; set; }

        [JsonProperty("price_str")] public string PriceStr { get; set; }

        public double Price { get; set; }

        public long Id { get; set; }

        //[JsonConverter(typeof(LiveOrdersStringConverter))]
        public long Datetime { get; set; }
    }
}