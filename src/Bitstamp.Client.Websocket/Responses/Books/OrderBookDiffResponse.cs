using System.Linq;
using System.Reactive.Subjects;
using Bitstamp.Client.Websocket.Json;
using Bitstamp.Client.Websocket.Messages;
using Newtonsoft.Json.Linq;

namespace Bitstamp.Client.Websocket.Responses.Books
{
	/// <summary>
	/// Diff order book response L2 - full order book
	/// </summary>
	public class OrderBookDiffResponse : SymbolResponse
	{
		/// <summary>
		/// Order book diff data
		/// </summary>
		public OrderBookDiff Data { get; set; }

		internal static bool TryHandle(JObject response, ISubject<OrderBookDiffResponse> subject)
		{
			if (!response.IsForChannel(ChannelPrefixes.DiffOrderBook))
				return false;

			var parsed = response.ToObject<OrderBookDiffResponse>(BitstampJsonSerializer.Serializer)!;

			parsed.ParseSymbolFromChannel();
			subject.OnNext(parsed);
			return true;
		}
	}
}