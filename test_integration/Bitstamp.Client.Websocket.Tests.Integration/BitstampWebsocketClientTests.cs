using System;
using System.Threading;
using System.Threading.Tasks;
using Bitstamp.Client.Websocket.Channels;
using Bitstamp.Client.Websocket.Client;
using Bitstamp.Client.Websocket.Communicator;
using Bitstamp.Client.Websocket.Requests;
using Bitstamp.Client.Websocket.Responses;
using Xunit;

namespace Bitstamp.Client.Websocket.Tests.Integration
{
    public class BitstampWebsocketClientTests
    {
        private static readonly string API_KEY = "your_api_key";
        private static readonly string API_SECRET = "";

        [Fact]
        public async Task ConnectTest()
        {
            var url = BitstampValues.ApiWebsocketUrl;
            using (var communicator = new BitstampWebsocketCommunicator(url))
            {
                HeartbeatResponse received = null;
                var receivedEvent = new ManualResetEvent(false);

                using (var client = new BitstampWebsocketClient(communicator))
                {
                    client.Streams.HeartbeatStream.Subscribe(pong =>
                    {
                        received = pong;
                        receivedEvent.Set();
                    });

                    await communicator.Start();

                    client.Send(new SubscribeRequest("btcusd", Channel.Heartbeat));

                    receivedEvent.WaitOne(TimeSpan.FromSeconds(90));

                    Assert.NotNull(received);
                }
            }
        }
    }
}