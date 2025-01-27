using System.Reactive.Subjects;
using Bitstamp.Client.Websocket.Json;
using Bitstamp.Client.Websocket.Messages;
using Newtonsoft.Json.Linq;

namespace Bitstamp.Client.Websocket.Responses.Error
{
	/// <summary>
	/// Error messages: Most failure cases will cause an error message (a message with the type "error") to be emitted.
	/// This can be helpful for implementing a client or debugging issues.
	/// </summary>
	public class ErrorResponse : ResponseBase
	{
		public Error Data { get; set; }

		internal static bool TryHandle(JObject response, ISubject<ErrorResponse> subject)
		{
			if (!response.IsForEvent(ControlEventNames.Error))
				return false;

			var parsed = response.ToObject<ErrorResponse>(BitstampJsonSerializer.Serializer)!;
        
			subject.OnNext(parsed);
			return true;
		}
	}
}