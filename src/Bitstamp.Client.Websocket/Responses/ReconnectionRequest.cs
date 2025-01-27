using System.Reactive.Subjects;
using Bitstamp.Client.Websocket.Messages;
using Newtonsoft.Json.Linq;

namespace Bitstamp.Client.Websocket.Responses
{
	public class ReconnectionRequest : ResponseBase
	{
		internal static bool TryHandle(JObject response, ISubject<ReconnectionRequest> subject)
		{
			if (!response.IsForEvent(ControlEventNames.RequestReconnect))
				return false;

			subject.OnNext(new ReconnectionRequest());
			return true;
		}
	}
}