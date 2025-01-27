namespace Bitstamp.Client.Websocket.Messages
{
	internal static class ChannelPrefixes
	{
		public const string DetailOrderBook = "detail_order_book";
		public const string DiffOrderBook = "diff_order_book";
		public const string OrderBook = "order_book";
		public const string LiveOrders = "live_orders";
		public const string PrivateMyOrders = "private-my_orders";
		public const string PrivateMyTrades = "private-my_trades";
		public const string LiveTrades = "live_trades";
	}
}