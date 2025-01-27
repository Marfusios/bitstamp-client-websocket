using System.Linq;
using System.Reactive.Subjects;
using Bitstamp.Client.Websocket.Json;
using Bitstamp.Client.Websocket.Messages;
using Newtonsoft.Json.Linq;

namespace Bitstamp.Client.Websocket.Responses.Books
{
	/// <summary>
	/// Full order book response L2 - first 100 levels on both sides
	/// </summary>
	public class OrderBookResponse : SymbolResponse
	{
		/// <summary>
		/// Order book data
		/// </summary>
		public OrderBook Data { get; set; }

		internal static bool TryHandle(JObject response, ISubject<OrderBookResponse> subject)
		{
			if (!response.IsForChannel(ChannelPrefixes.OrderBook))
				return false;

			var parsed = response.ToObject<OrderBookResponse>(BitstampJsonSerializer.Serializer)!;

			parsed.ParseSymbolFromChannel();
			subject.OnNext(parsed);
			return true;
		}
	}
}