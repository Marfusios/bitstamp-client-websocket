using Microsoft.Extensions.Logging;
using System;
using System.Net.WebSockets;
using Websocket.Client;

namespace Bitstamp.Client.Websocket.Communicator
{
    /// <inheritdoc cref="WebsocketClient" />
    public class BitstampWebsocketCommunicator : WebsocketClient, IBitstampCommunicator
    {
        /// <inheritdoc />
        public BitstampWebsocketCommunicator(Uri url, Func<ClientWebSocket>? clientFactory = null)
            : base(url, clientFactory)
        {
        }

        /// <inheritdoc />
        public BitstampWebsocketCommunicator(Uri url, ILogger<BitstampWebsocketCommunicator> logger, Func<ClientWebSocket>? clientFactory = null)
            : base(url, logger, clientFactory)
        {
        }
    }
}