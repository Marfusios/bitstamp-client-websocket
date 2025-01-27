using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Bitstamp.Client.Websocket.Responses;
using Bitstamp.Client.Websocket.Responses.Books;
using Bitstamp.Client.Websocket.Responses.Error;
using Bitstamp.Client.Websocket.Responses.Orders;
using Bitstamp.Client.Websocket.Responses.Trades;

namespace Bitstamp.Client.Websocket.Client
{
    /// <summary>
    /// All provided streams.
    /// You need to send subscription request in advance (via method `Send()` on BitstampWebsocketClient)
    /// </summary>
    public class BitstampClientStreams
    {
        internal readonly Subject<ErrorResponse> ErrorSubject = new Subject<ErrorResponse>();

        internal readonly Subject<HeartbeatResponse> HeartbeatSubject = new Subject<HeartbeatResponse>();

        internal readonly Subject<ReconnectionRequest> ReconnectionRequestSubject = new Subject<ReconnectionRequest>();

		internal readonly Subject<OrderBookDetailResponse> OrderBookDetailSubject =
            new Subject<OrderBookDetailResponse>();

        internal readonly Subject<OrderBookDiffResponse> OrderBookDiffSubject = new Subject<OrderBookDiffResponse>();

        internal readonly Subject<OrderBookResponse> OrderBookSubject = new Subject<OrderBookResponse>();
		
        internal readonly Subject<OrderResponse> OrdersSubject = new Subject<OrderResponse>();

        internal readonly Subject<PrivateOrderResponse> PrivateOrdersSubject = new Subject<PrivateOrderResponse>();

		internal readonly Subject<SubscriptionSucceeded> SubscriptionSucceededSubject =
            new Subject<SubscriptionSucceeded>();

        internal readonly Subject<UnsubscriptionSucceeded> UnsubscriptionSucceededSubject =
            new Subject<UnsubscriptionSucceeded>();

        internal readonly Subject<TradeResponse> TickerSubject = new Subject<TradeResponse>();

        internal readonly Subject<PrivateTradeResponse> PrivateTickerSubject = new Subject<PrivateTradeResponse>();

		// PUBLIC

		/// <summary>
		/// Server errors stream.
		/// Error messages: Most failure cases will cause an error message to be emitted.
		/// This can be helpful for implementing a client or debugging issues.
		/// </summary>
		public IObservable<ErrorResponse> ErrorStream => ErrorSubject.AsObservable();

        /// <summary>
        /// Response stream to every ping request
        /// </summary>
        public IObservable<HeartbeatResponse> HeartbeatStream => HeartbeatSubject.AsObservable();

        /// <summary>
        /// When Bitstamp wants us to reconnect
        /// </summary>
        public IObservable<ReconnectionRequest> ReconnectionRequestStream => ReconnectionRequestSubject.AsObservable();

		/// <summary>
		/// Subscription info stream, emits status after sending subscription request
		/// </summary>
		public IObservable<OrderResponse> OrdersStream => OrdersSubject.AsObservable();

        /// <summary>
        /// Subscription info stream, emits status after sending subscription request
        /// </summary>
        public IObservable<PrivateOrderResponse> PrivateOrdersStream => PrivateOrdersSubject.AsObservable();

		/// <summary>
		/// Executed trades
		/// </summary>
		public IObservable<TradeResponse> TickerStream => TickerSubject.AsObservable();

        /// <summary>
        /// Executed trades
        /// </summary>
        public IObservable<PrivateTradeResponse> PrivateTickerStream => PrivateTickerSubject.AsObservable();

		/// <summary>
		/// Order book stream L2 - first 100 levels on both sides
		/// </summary>
		public IObservable<OrderBookResponse> OrderBookStream => OrderBookSubject.AsObservable();

        /// <summary>
        /// Order book stream L3 - first 100 orders on both sides (contains order id)
        /// </summary>
        public IObservable<OrderBookDetailResponse> OrderBookDetailStream => OrderBookDetailSubject.AsObservable();

        /// <summary>
        /// Order book stream L2 - only diffs, full order book
        /// </summary>
        public IObservable<OrderBookDiffResponse> OrderBookDiffStream => OrderBookDiffSubject.AsObservable();

        /// <summary>
        /// Subscription stream
        /// </summary>
        public IObservable<SubscriptionSucceeded> SubscriptionSucceededStream =>
            SubscriptionSucceededSubject.AsObservable();

        /// <summary>
        /// Unsubscribe stream
        /// </summary>
        public IObservable<UnsubscriptionSucceeded> UnsubscriptionSucceededStream =>
            UnsubscriptionSucceededSubject.AsObservable();
    }
}