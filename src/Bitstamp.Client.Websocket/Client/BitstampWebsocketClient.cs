using System;
using Bitstamp.Client.Websocket.Json;
using Bitstamp.Client.Websocket.Requests;
using Bitstamp.Client.Websocket.Responses;
using Bitstamp.Client.Websocket.Responses.Books;
using Bitstamp.Client.Websocket.Responses.Error;
using Bitstamp.Client.Websocket.Responses.Orders;
using Bitstamp.Client.Websocket.Responses.Trades;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Websocket.Client;

namespace Bitstamp.Client.Websocket.Client;

/// <inheritdoc />
public class BitstampWebsocketClient : IBitstampWebsocketClient
{
    readonly ILogger _logger;
    readonly IWebsocketClient _client;
    readonly IDisposable _messageReceivedSubscription;

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    /// <param name="logger">The logger to use for logging any warnings or errors.</param>
    /// <param name="client">The client to use for the trade websocket.</param>
    public BitstampWebsocketClient(ILogger logger, IWebsocketClient client)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _client = client ?? throw new ArgumentNullException(nameof(client));

        _messageReceivedSubscription = _client.MessageReceived.Subscribe(HandleMessage);
    }

    /// <inheritdoc />
    public BitstampClientStreams Streams { get; } = new();

    /// <summary>
    /// Cleanup everything.
    /// </summary>
    public void Dispose() => _messageReceivedSubscription?.Dispose();

    /// <inheritdoc />
    public void Send<T>(T request) where T : RequestBase
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        try
        {
            var serialized = BitstampJsonSerializer.Serialize(request);

            _client.Send(serialized);
        }
        catch (Exception e)
        {
            _logger.LogError(e, LogMessage($"Exception while sending message '{request}'. Error: {e.Message}"));
            throw;
        }
    }

    static string LogMessage(string message) => $"[BITSTAMP WEBSOCKET CLIENT] {message}";

    void HandleMessage(ResponseMessage message)
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

            _logger.LogWarning(LogMessage($"Unhandled response:  '{messageSafe}'"));
        }
        catch (Exception e)
        {
            _logger.LogError(e, LogMessage("Exception while receiving message"));
        }
    }

    static bool HandleRawMessage(string message) => false;

    bool HandleObjectMessage(string message)
    {
        var response = BitstampJsonSerializer.Deserialize<JObject>(message);

        return
            SubscriptionSucceeded.TryHandle(response, Streams.SubscriptionSucceededStream) ||
            UnsubscriptionSucceeded.TryHandle(response, Streams.UnsubscriptionSucceededStream) ||
            TradeResponse.TryHandle(response, Streams.TickerStream) ||
            OrderBookResponse.TryHandle(response, Streams.OrderBookStream) ||
            OrderBookDetailResponse.TryHandle(response, Streams.OrderBookDetailStream) ||
            OrderBookDiffResponse.TryHandle(response, Streams.OrderBookDiffStream) ||
            ErrorResponse.TryHandle(response, Streams.ErrorStream) ||
            OrderResponse.TryHandle(response, Streams.OrdersStream) ||
            PrivateOrderResponse.TryHandle(response, Streams.PrivateOrdersStream) ||
            PrivateTradeResponse.TryHandle(response, Streams.PrivateTickerStream) ||
            HeartbeatResponse.TryHandle(response, Streams.HeartbeatStream) ||
            ReconnectionRequest.TryHandle(response, Streams.ReconnectionRequestStream);
    }
}