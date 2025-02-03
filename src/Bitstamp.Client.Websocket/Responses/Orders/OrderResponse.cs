using System.Reactive.Subjects;
using Bitstamp.Client.Websocket.Json;
using Bitstamp.Client.Websocket.Messages;
using Newtonsoft.Json.Linq;

namespace Bitstamp.Client.Websocket.Responses.Orders
{
	public class OrderResponse : SymbolResponse
	{
		public Order Data { get; set; }

		internal static bool TryHandle(JObject response, ISubject<OrderResponse> subject)
		{
			if (!response.IsForChannel(ChannelPrefixes.LiveOrders))
				return false;

			var parsed = response.ToObject<OrderResponse>(BitstampJsonSerializer.Serializer)!;

			parsed.ParseSymbolFromChannel();
			subject.OnNext(parsed);
			return true;
		}
	}
}