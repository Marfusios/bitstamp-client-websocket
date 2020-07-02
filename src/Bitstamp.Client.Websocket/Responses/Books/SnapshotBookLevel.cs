namespace Bitstamp.Client.Websocket.Responses.Books
{
    /// <summary>
    /// One order book level
    /// </summary>
    public class SnapshotBookLevel
    {
        public double Price { get; set; }
        public double Amount { get; set; }
    }
}