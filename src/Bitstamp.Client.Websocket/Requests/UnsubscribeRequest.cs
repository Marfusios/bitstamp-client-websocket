﻿using Bitstamp.Client.Websocket.Channels;
using Bitstamp.Client.Websocket.Messages;

namespace Bitstamp.Client.Websocket.Requests
{
	/// <summary>
	/// Unsubscribe request
	/// </summary>
	public class UnsubscribeRequest : DataRequest
	{
		public UnsubscribeRequest(string pair, Channel channel) : base(pair, channel) { }

		/// <inheritdoc />
		public override string Event => ControlEventNames.Unsubscribe;
	}
}