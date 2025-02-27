﻿using Bitstamp.Client.Websocket.Channels;
using Bitstamp.Client.Websocket.Messages;

namespace Bitstamp.Client.Websocket.Requests
{
	/// <summary>
	/// Subscribe request
	/// </summary>
	public class SubscribeRequest : DataRequest
	{
		public SubscribeRequest(string pair, Channel channel) : base(pair, channel) { }

		/// <inheritdoc />
		public override string Event => ControlEventNames.Subscribe;
	}
}