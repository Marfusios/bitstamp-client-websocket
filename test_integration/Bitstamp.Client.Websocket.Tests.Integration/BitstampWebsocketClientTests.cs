using System;
using System.Threading;
using System.Threading.Tasks;
using Bitstamp.Client.Websocket.Client;
using Bitstamp.Client.Websocket.Requests;
using Bitstamp.Client.Websocket.Responses;
using Microsoft.Extensions.Logging.Abstractions;
using Websocket.Client;
using Xunit;

namespace Bitstamp.Client.Websocket.Tests.Integration;

public class BitstampWebsocketClientTests
{
    [Fact]
    public async Task ConnectTest()
    {
        var url = BitstampValues.ApiWebsocketUrl;
        using var apiClient = new WebsocketClient(url);
        HeartbeatResponse received = null;
        var receivedEvent = new ManualResetEvent(false);

        using var client = new BitstampWebsocketClient(NullLogger.Instance, apiClient);
        client.Streams.HeartbeatStream.Subscribe(pong =>
        {
            received = pong;
            receivedEvent.Set();
        });

        await apiClient.Start();

        client.Send(new HeartbeatRequest());

        receivedEvent.WaitOne(TimeSpan.FromSeconds(90));

        Assert.NotNull(received);
    }
}