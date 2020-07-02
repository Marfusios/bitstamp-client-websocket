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
    public class OrderBookSnapshotResponse : ResponseBase
    {
        [JsonProperty("timestamp")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Timestamp { get; set; }

        [JsonProperty("microtimestamp")] public string Microtimestamp { get; set; }

        /// <summary>
        /// Order book bid levels
        /// </summary>
        //[JsonConverter(typeof(OrderBookLevelConverter), OrderBookSide.Buy)]
        [JsonProperty("bids")]
        public SnapshotBookLevel[] Bids { get; set; }

        /// <summary>
        /// Order book ask levels
        /// </summary>
        //[JsonConverter(typeof(OrderBookLevelConverter), OrderBookSide.Sell)]
        [JsonProperty("asks")]
        public SnapshotBookLevel[] Asks { get; set; }

        internal static bool TryHandle(JObject response, ISubject<OrderBookSnapshotResponse> subject)
        {
            /*
            
            if (stream == null)
                return false;

            if (!stream.Contains("depth"))
                return false;

            if (stream.EndsWith("depth"))
            {
                // ignore, not partial, but diff response
                return false;
            }*/
            //var stream = response?["stream"]?.Value<string>();

            if (response != null && (bool) !response?["channel"].HasValues)
            {
                var parsedSnapshot = response?.ToObject<OrderBookSnapshotResponse>(BitstampJsonSerializer.Serializer);
                //parsed.Symbol = response?["channel"].Value<string>().Split('_')[index];
                subject.OnNext(parsedSnapshot);
                return true;
            }

            return true;
        }

        /// <summary>
        /// Stream snapshot manually via communicator
        /// </summary>
        public static void StreamFakeSnapshot(OrderBookSnapshot snapshot, IBitstampCommunicator communicator)
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

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t)
        {
            return t == typeof(long) || t == typeof(long?);
        }

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (long.TryParse(value, out l)) return l;
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }

            var value = (long) untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}