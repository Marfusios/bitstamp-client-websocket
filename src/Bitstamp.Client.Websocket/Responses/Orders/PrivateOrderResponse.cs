using System.Reactive.Subjects;
using Bitstamp.Client.Websocket.Json;
using Bitstamp.Client.Websocket.Messages;
using Newtonsoft.Json.Linq;

namespace Bitstamp.Client.Websocket.Responses.Orders
{
	public class PrivateOrderResponse : SymbolResponse
	{
		public PrivateOrder Data { get; set; }

		internal static bool TryHandle(JObject response, ISubject<PrivateOrderResponse> subject)
		{
			if (!response.IsForChannel(ChannelPrefixes.PrivateMyOrders))
				return false;

			var parsed = response.ToObject<PrivateOrderResponse>(BitstampJsonSerializer.Serializer)!;

			parsed.ParseSymbolFromChannel();
			subject.OnNext(parsed);
			return true;
		}
	}
}