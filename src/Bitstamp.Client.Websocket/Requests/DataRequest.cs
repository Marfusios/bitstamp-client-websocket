using System;
using Bitstamp.Client.Websocket.Channels;
using Bitstamp.Client.Websocket.Messages;
using Bitstamp.Client.Websocket.Utils;

namespace Bitstamp.Client.Websocket.Requests;

/// <summary>
/// Base class for public requests
/// </summary>
public abstract class DataRequest : RequestBase
{
    readonly string _pair;
    readonly PublicChannel _channel;

    protected DataRequest(string pair, PublicChannel channel)
    {
        _pair = pair;
        _channel = channel;
    }

    public RequestData Data => new()
    {
        Channel = $"{ChannelPrefix}_{CryptoPairsHelper.Clean(_pair)}"
    };

    string ChannelPrefix => _channel switch
    {
        PublicChannel.Ticker => ChannelPrefixes.LiveTrades,
        PublicChannel.Orders => ChannelPrefixes.LiveOrders,
        PublicChannel.OrderBook => ChannelPrefixes.OrderBook,
        PublicChannel.OrderBookDetail => ChannelPrefixes.DetailOrderBook,
        PublicChannel.OrderBookDiff => ChannelPrefixes.DiffOrderBook,
        _ => throw new NotImplementedException()
    };
}