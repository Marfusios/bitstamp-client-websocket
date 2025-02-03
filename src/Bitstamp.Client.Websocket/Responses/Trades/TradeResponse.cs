using System.Linq;
using System.Reactive.Subjects;
using Bitstamp.Client.Websocket.Json;
using Bitstamp.Client.Websocket.Messages;
using Newtonsoft.Json.Linq;

namespace Bitstamp.Client.Websocket.Responses.Trades
{
	/// <summary>
	/// Trade (ticker) response
	/// </summary>
	public class TradeResponse : SymbolResponse
	{
		/// <summary>
		/// Trade/ticker data
		/// </summary>
		public Trade Data { get; set; }

		internal static bool TryHandle(JObject response, ISubject<TradeResponse> subject)
		{
			if (!response.IsForChannel(ChannelPrefixes.LiveTrades))
				return false;

			var parsed = response.ToObject<TradeResponse>(BitstampJsonSerializer.Serializer)!;

			parsed.ParseSymbolFromChannel();
			subject.OnNext(parsed);
			return true;
		}
	}
}