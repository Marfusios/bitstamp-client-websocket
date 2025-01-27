using System;
using Bitstamp.Client.Websocket.Json;
using Newtonsoft.Json;

namespace Bitstamp.Client.Websocket.Responses.Trades
{
	/// <summary>
	/// Trade (ticker) data
	/// </summary>
	public class Trade
	{
		/// <summary>
		/// Trade unique id
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// Microtimestamp - datetime with milliseconds
		/// </summary>
		[JsonConverter(typeof(UnixDateTimeMillisecondsConverter))]
		public DateTime Microtimestamp { get; set; }

		/// <summary>
		/// Timestamp - datetime only seconds
		/// </summary>
		[JsonConverter(typeof(UnixDateTimeSecondsConverter))]
		public DateTime Timestamp { get; set; }

		/// <summary>
		/// Trade amount - size
		/// </summary>
		public double Amount { get; set; }

		/// <summary>
		/// Trade amount represented in string format
		/// </summary>
		public string AmountStr { get; set; }

		/// <summary>
		/// Trade buy order ID
		/// </summary>
		public long BuyOrderId { get; set; }

		/// <summary>
		/// Trade sell order ID.
		/// </summary>
		public long SellOrderId { get; set; }

		/// <summary>
		/// Trade price
		/// </summary>
		public double Price { get; set; }

		/// <summary>
		/// Trade price represented in string format
		/// </summary>
		public string PriceStr { get; set; }

		/// <summary>
		/// Trade type (0 - buy; 1 - sell)
		/// </summary>
		public int Type { get; set; }

		/// <summary>
		/// Trade side
		/// </summary>
		[JsonIgnore]
		public TradeSide Side => Type == 0
			? TradeSide.Buy
			: Type == 1
				? TradeSide.Sell
				: TradeSide.Undefined;
	}
}