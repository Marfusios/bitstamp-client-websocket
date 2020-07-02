using System;
using Bitstamp.Client.Websocket.Communicator;
using Bitstamp.Client.Websocket.Json;
using Bitstamp.Client.Websocket.Logging;
using Bitstamp.Client.Websocket.Requests;
using Bitstamp.Client.Websocket.Responses;
using Bitstamp.Client.Websocket.Responses.Books;
using Bitstamp.Client.Websocket.Responses.Orders;
using Bitstamp.Client.Websocket.Validations;
using Newtonsoft.Json.Linq;
using Websocket.Client;

namespace Bitstamp.Client.Websocket.Client
{
    /// <summary>
    /// Bitstamp websocket client.
    /// Use method `Send()` to subscribe to channels.
    /// And `Streams` to handle messages.
    /// </summary>
    public class BitstampWebsocketClient : IDisposable
    {
        private static readonly ILog Log = LogProvider.GetCurrentClassLogger();

        private readonly IBitstampCommunicator _communicator;
        private readonly IDisposable _messageReceivedSubscription;

        /// <summary>
        /// Bitstamp websocket client.
        /// Use method `Send()` to subscribe to channels.
        /// And `Streams` to handle messages.
        /// </summary>
        /// <param name="communicator">Live or backtest communicator</param>
        public BitstampWebsocketClient(IBitstampCommunicator communicator)
        {
            BitstampValidations.ValidateInput(communicator, nameof(communicator));

            _communicator = communicator;
            _messageReceivedSubscription = _communicator.MessageReceived.Subscribe(HandleMessage);
        }

        /// <summary>
        /// Provided message streams
        /// </summary>
        public BitstampClientStreams Streams { get; } = new BitstampClientStreams();

        /// <summary>
        /// Cleanup everything
        /// </summary>
        public void Dispose()
        {
            _messageReceivedSubscription?.Dispose();
        }

        /// <summary>
        /// Serializes request and sends message via websocket communicator.
        /// It logs and re-throws every exception.
        /// </summary>
        /// <param name="request">Request/message to be sent</param>
        public void Send<T>(T request) where T : RequestBase
        {
            try
            {
                BitstampValidations.ValidateInput(request, nameof(request));

                var serialized =
                    BitstampJsonSerializer.Serialize(request);

                _communicator.Send(serialized);
            }
            catch (Exception e)
            {
                Log.Error(e, L($"Exception while sending message '{request}'. Error: {e.Message}"));
                throw;
            }
        }

        private string L(string msg)
        {
            return $"[BITSTAMP WEBSOCKET CLIENT] {msg}";
        }

        private void HandleMessage(ResponseMessage message)
        {
            try
            {
                bool handled;
                var messageSafe = (message.Text ?? string.Empty).Trim();

                if (messageSafe.StartsWith("{"))
                {
                    handled = HandleObjectMessage(messageSafe);
                    if (handled) return;
                }

                handled = HandleRawMessage(messageSafe);
                if (handled) return;

                Log.Warn(L($"Unhandled response:  '{messageSafe}'"));
            }
            catch (Exception e)
            {
                Log.Error(e, L("Exception while receiving message"));
            }
        }

        private bool HandleRawMessage(string msg)
        {
            // ********************
            // ADD RAW HANDLERS BELOW
            // ********************

            return false;
        }

        private bool HandleObjectMessage(string msg)
        {
            var response = BitstampJsonSerializer.Deserialize<JObject>(msg);

            // ********************
            // ADD OBJECT HANDLERS BELOW
            // ********************

            return
                SubscriptionSucceeded.TryHandle(response, Streams.SubscriptionSucceededSubject) ||
                UnsubscriptionSucceeded.TryHandle(response, Streams.UnsubscriptionSucceededSubject) ||
                //OrderBookSnapshotResponse.TryHandle(response, Streams.OrderBookSnapshotSubject) ||
                Ticker.TryHandle(response, Streams.TickerSubject) ||
                OrderBookResponse.TryHandle(response, Streams.OrderBookSubject) ||
                OrderBookDetailResponse.TryHandle(response, Streams.OrderBookDetailSubject) ||
                OrderBookDiffResponse.TryHandle(response, Streams.OrderBookDiffSubject) ||
                ErrorResponse.TryHandle(response, Streams.ErrorSubject) ||
                OrderResponse.TryHandle(response, Streams.OrdersSubject) ||
                false;
        }
    }
}