using System.Reactive.Subjects;
using Bitstamp.Client.Websocket.Json;
using Bitstamp.Client.Websocket.Messages;
using Newtonsoft.Json.Linq;

namespace Bitstamp.Client.Websocket.Responses.Trades
{
	public class PrivateTradeResponse : SymbolResponse
	{
		public PrivateTrade Data { get; set; }

		internal static bool TryHandle(JObject response, ISubject<PrivateTradeResponse> subject)
		{
			if (!response.IsForChannel(ChannelPrefixes.PrivateMyTrades))
				return false;

			var parsed = response.ToObject<PrivateTradeResponse>(BitstampJsonSerializer.Serializer)!;

			parsed.ParseSymbolFromChannel();
			subject.OnNext(parsed);
			return true;
		}
	}
}