using Bitstamp.Client.Websocket.Messages;

namespace Bitstamp.Client.Websocket.Requests
{
	/// <summary>
	/// Heartbeat request
	/// </summary>
	public class HeartbeatRequest : RequestBase
	{
		/// <inheritdoc />
		public override string Event => ControlEventNames.Heartbeat;
	}
}