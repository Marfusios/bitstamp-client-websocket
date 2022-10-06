using System.Reactive.Subjects;
using Bitstamp.Client.Websocket.Responses;
using Bitstamp.Client.Websocket.Responses.Books;
using Bitstamp.Client.Websocket.Responses.Error;
using Bitstamp.Client.Websocket.Responses.Orders;
using Bitstamp.Client.Websocket.Responses.Trades;

namespace Bitstamp.Client.Websocket.Client;

/// <summary>
/// All provided streams.
/// You need to send subscription request in advance (via method `Send()` on BitstampWebsocketClient)
/// </summary>
public class BitstampClientStreams
{
    // PUBLIC

    /// <summary>
    /// Server errors stream.
    /// Error messages: Most failure cases will cause an error message to be emitted.
    /// This can be helpful for implementing a client or debugging issues.
    /// </summary>
    public readonly Subject<ErrorResponse> ErrorStream = new();

    /// <summary>
    /// Response stream to every ping request
    /// </summary>
    public readonly Subject<HeartbeatResponse> HeartbeatStream = new();

    /// <summary>
    /// Order changes (insert/update/delete)
    /// </summary>
    public readonly Subject<OrderResponse> OrdersStream = new();

    /// <summary>
    /// Executed trades
    /// </summary>
    public readonly Subject<TradeResponse> TickerStream = new();

    /// <summary>
    /// Order book stream L2 - first 100 levels on both sides
    /// </summary>
    public readonly Subject<OrderBookResponse> OrderBookStream = new();

    /// <summary>
    /// Order book stream L3 - first 100 orders on both sides (contains order id)
    /// </summary>
    public readonly Subject<OrderBookDetailResponse> OrderBookDetailStream = new();

    /// <summary>
    /// Order book stream L2 - only diffs, full order book
    /// </summary>
    public readonly Subject<OrderBookDiffResponse> OrderBookDiffStream = new();

    /// <summary>
    /// Subscription stream
    /// </summary>
    public readonly Subject<SubscriptionSucceeded> SubscriptionSucceededStream = new();

    /// <summary>
    /// Unsubscribe stream
    /// </summary>
    public readonly Subject<UnsubscriptionSucceeded> UnsubscriptionSucceededStream = new();

    /// <summary>
    /// Reconnection request stream
    /// </summary>
    public readonly Subject<ReconnectionRequest> ReconnectionRequestStream = new();


    // PRIVATE

    /// <summary>
    /// Private order changes (insert/update/delete)
    /// </summary>
    public readonly Subject<PrivateOrderResponse> PrivateOrdersStream = new();

    /// <summary>
    /// Private executed trades
    /// </summary>
    public readonly Subject<PrivateTradeResponse> PrivateTickerStream = new();
}