using System.Linq;

namespace Bitstamp.Client.Websocket.Responses
{
	/// <summary>
	/// Message which is used as base for responses related to a trading pair
	/// </summary>
	public abstract class SymbolResponse : ResponseBase
	{
		public string Symbol { get; set; }

		internal void ParseSymbolFromChannel() => Symbol = Channel.Split('_').Last().Split('-')[0];
	}
}