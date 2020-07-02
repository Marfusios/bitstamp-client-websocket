using System;
using System.Threading;
using System.Threading.Tasks;
using Bitstamp.Client.Websocket.Communicator;
using Xunit;

namespace Bitstamp.Client.Websocket.Tests.Integration
{
    public class BitstampWebsocketCommunicatorTests
    {
        [Fact]
        public async Task OnStarting_ShouldGetInfoResponse()
        {
            var url = BitstampValues.ApiWebsocketUrl;
            using (var communicator = new BitstampWebsocketCommunicator(url))
            {
                string received = null;
                var receivedEvent = new ManualResetEvent(false);

                communicator.MessageReceived.Subscribe(msg =>
                {
                    received = msg.Text;
                    receivedEvent.Set();
                });

                await communicator.Start();

                communicator.Send("invalid test request");

                receivedEvent.WaitOne(TimeSpan.FromSeconds(30));

                Assert.NotNull(received);
            }
        }
    }
}