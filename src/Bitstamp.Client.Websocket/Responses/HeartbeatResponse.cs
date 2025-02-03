using System.Reactive.Subjects;
using Bitstamp.Client.Websocket.Json;
using Bitstamp.Client.Websocket.Messages;
using Newtonsoft.Json.Linq;

namespace Bitstamp.Client.Websocket.Responses
{
	/// <summary>
	/// Heartbeat response
	/// </summary>
	public class HeartbeatResponse : ResponseBase
	{
		internal static bool TryHandle(JObject response, ISubject<HeartbeatResponse> subject)
		{
			if (!response.IsForEvent(ControlEventNames.Heartbeat))
				return false;

			var parsed = response.ToObject<HeartbeatResponse>(BitstampJsonSerializer.Serializer)!;

			subject.OnNext(parsed);
			return true;
		}
	}
}