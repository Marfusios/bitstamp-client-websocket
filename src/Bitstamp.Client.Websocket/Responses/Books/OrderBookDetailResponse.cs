using System.Linq;
using System.Reactive.Subjects;
using Bitstamp.Client.Websocket.Json;
using Bitstamp.Client.Websocket.Messages;
using Newtonsoft.Json.Linq;

namespace Bitstamp.Client.Websocket.Responses.Books
{
	/// <summary>
	/// Full order book response L3 - first 100 orders on both sides
	/// </summary>
	public class OrderBookDetailResponse : SymbolResponse
	{
		/// <summary>
		/// Order book detail data
		/// </summary>
		public OrderBookDetail Data { get; set; }

		internal static bool TryHandle(JObject response, ISubject<OrderBookDetailResponse> subject)
		{
			if (!response.IsForChannel(ChannelPrefixes.DetailOrderBook))
				return false;

			var parsed = response.ToObject<OrderBookDetailResponse>(BitstampJsonSerializer.Serializer)!;

			parsed.ParseSymbolFromChannel();
			subject.OnNext(parsed);
			return true;
		}
	}
}