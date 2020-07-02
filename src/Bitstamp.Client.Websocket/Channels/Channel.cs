namespace Bitstamp.Client.Websocket.Channels
{
    /// <summary>
    /// Bitstamp websocket channels
    /// </summary>
    public enum Channel
    {
        /// <summary>
        /// Heartbeat
        /// </summary>
        Heartbeat,

        /// <summary>
        /// Ticker
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
}