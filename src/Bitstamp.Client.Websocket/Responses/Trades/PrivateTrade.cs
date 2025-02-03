using System;
using Bitstamp.Client.Websocket.Json;
using Newtonsoft.Json;

namespace Bitstamp.Client.Websocket.Responses.Trades
{
	/// <summary>
	/// Private trade (ticker) data
	/// </summary>
	public class PrivateTrade
	{
		/// <summary>
		/// Trade ID
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// Order ID associated with the trade
		/// </summary>
		public long OrderId { get; set; }

		/// <summary>
		/// Client order ID associated with the trade
		/// </summary>
		public string ClientOrderId { get; set; }

		/// <summary>
		/// Trade amount
		/// </summary>
		public double Amount { get; set; }

		/// <summary>
		/// Trade price
		/// </summary>
		public double Price { get; set; }

		/// <summary>
		/// Trade fee
		/// </summary>
		public double Fee { get; set; }

		/// <summary>
		/// Trade side (buy or sell)
		/// </summary>
		public TradeSide Side { get; set; }

		/// <summary>
		/// Microtimestamp - datetime with milliseconds
		/// </summary>
		[JsonConverter(typeof(UnixDateTimeMillisecondsConverter))]
		public DateTime Microtimestamp { get; set; }
	}
}