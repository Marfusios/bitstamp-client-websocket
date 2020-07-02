using System.Runtime.Serialization;

namespace Bitstamp.Client.Websocket.Messages
{
    public enum MessageType
    {
        // Do not rename, used in requests
        [DataMember(Name = "bts:subscribe")] Subscribe,
        [DataMember(Name = "bts:unsubscribe")] Unsubscribe,

        // Can be renamed, only for responses
        Error,
        Info,
        Trade,
        OrderBook,
        Wallet,
        Order,
        Position,
        Quote,
        Instrument,
        Margin,
        Execution,
        Funding,
        OrderBookDiff,
        OrderBookDetail,
        Snapshot
    }
}