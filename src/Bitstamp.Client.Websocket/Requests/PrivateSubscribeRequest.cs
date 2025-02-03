using Bitstamp.Client.Websocket.Channels;
using Bitstamp.Client.Websocket.Messages;

namespace Bitstamp.Client.Websocket.Requests
{
	/// <summary>
	/// Private subscribe request
	/// </summary>
	public class PrivateSubscribeRequest : PrivateDataRequest
	{
		public PrivateSubscribeRequest(string pair, Channel channel) : base(pair, channel) { }

		/// <inheritdoc />
		public override string Event => ControlEventNames.Subscribe;
	}
}