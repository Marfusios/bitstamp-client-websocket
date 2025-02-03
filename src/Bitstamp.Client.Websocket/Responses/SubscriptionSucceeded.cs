using System.Reactive.Subjects;
using Bitstamp.Client.Websocket.Json;
using Bitstamp.Client.Websocket.Messages;
using Newtonsoft.Json.Linq;

namespace Bitstamp.Client.Websocket.Responses
{
	public class SubscriptionSucceeded : SymbolResponse
	{
		internal static bool TryHandle(JObject response, ISubject<SubscriptionSucceeded> subject)
		{
			if (!response.IsForEvent(ControlEventNames.SubscriptionSucceeded))
				return false;

			var parsed = response.ToObject<SubscriptionSucceeded>(BitstampJsonSerializer.Serializer)!;

			parsed.ParseSymbolFromChannel();
			subject.OnNext(parsed);
			return true;
		}
	}
}