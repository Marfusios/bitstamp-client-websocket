using System;
using System.Threading;
using System.Threading.Tasks;
using Bitstamp.Client.Websocket.Channels;
using Bitstamp.Client.Websocket.Client;
using Bitstamp.Client.Websocket.Communicator;
using Bitstamp.Client.Websocket.Requests;
using Bitstamp.Client.Websocket.Responses.Books;
using Xunit;

namespace Bitstamp.Client.Websocket.Tests.Integration
{
    public class BitstampWebsocketClientTests
    {
        private static readonly string ApiKey = "your_api_key";
        private static readonly string ApiSecret = "";

        [Fact]
        public async Task ConnectTest()
        {
            var url = BitstampValues.ApiWebsocketUrl;
            using (var communicator = new BitstampWebsocketCommunicator(url))
            {
                OrderBookResponse received = null;
                var receivedEvent = new ManualResetEvent(false);

                using (var client = new BitstampWebsocketClient(communicator))
                {
                    client.Streams.OrderBookStream.Subscribe(pong =>
                    {
                        received = pong;
                        receivedEvent.Set();
                    });

                    await communicator.Start();

                    client.Send(new SubscribeRequest("btcusd", Channel.OrderBook));

                    receivedEvent.WaitOne(TimeSpan.FromSeconds(20));

                    Assert.NotNull(received);
                }
            }
        }
    }
}