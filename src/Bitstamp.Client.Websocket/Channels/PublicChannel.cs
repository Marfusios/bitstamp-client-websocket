namespace Bitstamp.Client.Websocket.Channels;

/// <summary>
/// Bitstamp public websocket channels
/// </summary>
public enum PublicChannel
{
    /// <summary>
    /// Trades
    /// </summary>
    Ticker,

    /// <summary>
    /// Orders
    /// </summary>
    Orders,

    /// <summary>
    /// OrderBook
    /// </summary>
    OrderBook,

    /// <summary>
    /// OrderBookDetail
    /// </summary>
    OrderBookDetail,

    /// <summary>
    /// OrderBookDiff
    /// </summary>
    OrderBookDiff
}