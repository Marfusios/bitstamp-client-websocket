using Newtonsoft.Json.Linq;

namespace Bitstamp.Client.Websocket.Messages
{
	internal static class JObjectExtensions
	{
		public static bool IsForChannel(this JObject jObject, string channelPrefix) => jObject["channel"]!.Value<string>()?.StartsWith(channelPrefix) == true;

		public static bool IsForEvent(this JObject jObject, string eventName) => jObject["event"]!.Value<string>() == eventName;
	}
}