using System;
using Bitstamp.Client.Websocket.Json;
using Newtonsoft.Json;

namespace Bitstamp.Client.Websocket.Responses.Orders
{
	public class PrivateOrder
	{
		/// <summary>
		/// Order ID
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// Order ID represented in string format
		/// </summary>
		public string IdStr { get; set; }

		/// <summary>
		/// Client order ID (if used when placing order)
		/// </summary>
		public string ClientOrderId { get; set; }

		/// <summary>
		/// Order amount
		/// </summary>
		public double Amount { get; set; }

		/// <summary>
		/// Order amount represented in string format
		/// </summary>
		public string AmountStr { get; set; }

		/// <summary>
		/// Order price
		/// </summary>
		public double Price { get; set; }

		/// <summary>
		/// Order price represented in string format
		/// </summary>
		public string PriceStr { get; set; }

		/// <summary>
		/// Order type (0 - buy, 1 - sell)
		/// </summary>
		public int OrderType { get; set; }

		/// <summary>
		/// Order datetime
		/// </summary>
		[JsonConverter(typeof(UnixDateTimeSecondsConverter))]
		public DateTime Datetime { get; set; }

		/// <summary>
		/// Order action timestamp represented in microseconds
		/// </summary>
		[JsonConverter(typeof(UnixDateTimeMillisecondsConverter))]
		public DateTime Microtimestamp { get; set; }
	}
}