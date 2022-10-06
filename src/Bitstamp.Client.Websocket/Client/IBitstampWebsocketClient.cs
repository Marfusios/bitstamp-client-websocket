using System;
using Bitstamp.Client.Websocket.Requests;

namespace Bitstamp.Client.Websocket.Client;

/// <summary>
/// Bitstamp websocket client.
/// Use method `Send()` to subscribe to channels.
/// And `Streams` to handle messages.
/// </summary>
public interface IBitstampWebsocketClient : IDisposable
{
    /// <summary>
    /// Serializes request and sends message via websocket client.
    /// It logs and re-throws every exception.
    /// </summary>
    /// <param name="request">Request/message to be sent</param>
    void Send<T>(T request) where T : RequestBase;

    /// <summary>
    /// Provided message streams.
    /// </summary>
    BitstampClientStreams Streams { get; }
}