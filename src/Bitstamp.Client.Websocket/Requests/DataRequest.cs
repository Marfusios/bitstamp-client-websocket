using System;
using Bitstamp.Client.Websocket.Channels;
using Bitstamp.Client.Websocket.Messages;
using Bitstamp.Client.Websocket.Utils;

namespace Bitstamp.Client.Websocket.Requests
{
	/// <summary>
	/// Base class for public requests
	/// </summary>
	public abstract class DataRequest : RequestBase
	{
		private readonly string _pair;
		private readonly Channel _channel;

		protected DataRequest(string pair, Channel channel)
		{
			_pair = pair;
			_channel = channel;
		}

		public RequestData Data => new RequestData()
		{
			Channel = $"{ChannelPrefix}_{CryptoPairsHelper.Clean(_pair)}"
		};

		string ChannelPrefix => _channel switch
		{
			Channel.Ticker => ChannelPrefixes.LiveTrades,
			Channel.Orders => ChannelPrefixes.LiveOrders,
			Channel.OrderBook => ChannelPrefixes.OrderBook,
			Channel.OrderBookDetail => ChannelPrefixes.DetailOrderBook,
			Channel.OrderBookDiff => ChannelPrefixes.DiffOrderBook,
			_ => throw new NotImplementedException()
		};
	}
}