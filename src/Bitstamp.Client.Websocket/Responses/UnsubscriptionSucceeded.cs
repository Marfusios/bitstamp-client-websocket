using System.Reactive.Subjects;
using Bitstamp.Client.Websocket.Json;
using Bitstamp.Client.Websocket.Messages;
using Newtonsoft.Json.Linq;

namespace Bitstamp.Client.Websocket.Responses
{
	public class UnsubscriptionSucceeded : SymbolResponse
	{
		internal static bool TryHandle(JObject response, ISubject<UnsubscriptionSucceeded> subject)
		{
			if (!response.IsForEvent(ControlEventNames.UnsubscriptionSucceeded))
				return false;

			var parsed = response.ToObject<UnsubscriptionSucceeded>(BitstampJsonSerializer.Serializer)!;

			parsed.ParseSymbolFromChannel();
			subject.OnNext(parsed);
			return true;
		}
	}
}