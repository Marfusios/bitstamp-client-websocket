using System;
using System.Globalization;
using Bitstamp.Client.Websocket.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Bitstamp.Client.Websocket.Json
{
    internal class UnixDateTimeSecondsConverter : DateTimeConverterBase
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var subtracted = ((DateTime) value).Subtract(UnixTime.UnixBase);
            writer.WriteRawValue(subtracted.TotalSeconds.ToString(CultureInfo.InvariantCulture));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (reader.Value == null) 
                return null;

            if (reader.Value is string valueS && long.TryParse(valueS, out var valueSl))
                return UnixTime.ConvertToTimeFromSeconds(valueSl);

            if (reader.Value is long valueL)
                return UnixTime.ConvertToTimeFromSeconds(valueL);

            return null;
        }
    }

    internal class UnixDateTimeMillisecondsConverter : DateTimeConverterBase
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var subtracted = ((DateTime)value).Subtract(UnixTime.UnixBase);
            writer.WriteRawValue(subtracted.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (reader.Value == null) 
                return null;

            if (reader.Value is string valueS && long.TryParse(valueS, out var valueSl))
                return UnixTime.ConvertToTimeFromMilliseconds(valueSl/1000);

            if (reader.Value is long valueL)
                return UnixTime.ConvertToTimeFromMilliseconds(valueL/1000);

            return null;
        }
    }
}