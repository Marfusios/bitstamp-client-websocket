﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using Websocket.Client.Sample.Models;
//
//    var liveOrders = Orders.FromJson(jsonString);

using System.Reactive.Subjects;
using Bitstamp.Client.Websocket.Json;
using Bitstamp.Client.Websocket.Messages;
using Newtonsoft.Json.Linq;

namespace Bitstamp.Client.Websocket.Responses.Orders
{
    public class OrderResponse : ResponseBase
    {
        public override MessageType Event => MessageType.Order;
        public Order Data { get; set; }

        internal static bool
            TryHandle(JObject response, ISubject<OrderResponse> subject)
        {
            //if (response?["channel"].Value<string>() != "order_created" ||
            //response?["channel"].Value<string>() != "order_changed" ||
            //response?["channel"].Value<string>() != "order_deleted") return false;

            if (response != null && (bool) !response?["channel"].Value<string>().Contains("live_orders")) return false;

            var parsed = response?.ToObject<OrderResponse>(BitstampJsonSerializer.Serializer);

            if (parsed != null)
            {
                parsed.Symbol = response?["channel"].Value<string>().Split('_')[2];

                subject.OnNext(parsed);
            }

            subject.OnNext(parsed);
            return true;
        }
    }
}

/*
    internal class LiveOrdersStringConverter : JsonConverter
    {
        public static readonly LiveOrdersStringConverter Singleton = new LiveOrdersStringConverter();

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
        }
    }
}*/