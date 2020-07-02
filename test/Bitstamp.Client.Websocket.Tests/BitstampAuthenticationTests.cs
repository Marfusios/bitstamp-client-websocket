using Bitstamp.Client.Websocket.Utils;
using Xunit;

namespace Bitstamp.Client.Websocket.Tests
{
    public class BitstampAuthenticationTests
    {
        [Fact]
        public void CreateSignature_ShouldReturnCorrectString()
        {
            var nonce = BitstampAuthentication.CreateAuthNonce(123456);
            var payload = BitstampAuthentication.CreateAuthPayload(nonce);
            var signature = BitstampAuthentication.CreateSignature(payload, "api_secret");

            Assert.Equal("f6bea0776d7db5b8f74bc930f5b8d6901376874cfc433cf4b68b688d78238e74", signature);
        }
    }
}