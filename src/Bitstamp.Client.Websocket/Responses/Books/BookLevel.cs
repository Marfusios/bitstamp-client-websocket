namespace Bitstamp.Client.Websocket.Responses.Books
{
    /// <summary>
    /// One order book level
    /// </summary>
    public class BookLevel
    {
        /// <summary>
        /// Order book level side
        /// </summary>
        public OrderBookSide Side { get; set; }

        /// <summary>
        /// Order book level price
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Order book level amount
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Order book order id (only available for L3 data - detail)
        /// </summary>
        public long OrderId { get; set; }
    }
}