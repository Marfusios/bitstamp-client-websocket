using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Bitstamp.Client.Websocket.Json
{
    /// <summary>
    /// Helper class for JSON serialization
    /// </summary>
    public static class BitstampJsonSerializer
    {
        /// <summary>
        /// Custom JSON settings
        /// </summary>
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.None,
            //FloatParseHandling = FloatParseHandling.decimal,
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
			},
			Converters = new List<JsonConverter>
			{
				new StringEnumConverter
				{
					NamingStrategy = new SnakeCaseNamingStrategy()
				}
            }
        };

        /// <summary>
        /// Custom preconfigured JSON serializer
        /// </summary>
        public static readonly JsonSerializer Serializer = JsonSerializer.Create(Settings);

        /// <summary>
        /// Deserialize JSON string data by our configuration
        /// </summary>
        public static T? Deserialize<T>(string data) where T : class
        {
            return JsonConvert.DeserializeObject<T>(data, Settings);
        }

        /// <summary>
        /// Serialize object into JSON by our configuration
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data, Settings);
        }
    }
}