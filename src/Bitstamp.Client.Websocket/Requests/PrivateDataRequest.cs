using System;
using Bitstamp.Client.Websocket.Channels;
using Bitstamp.Client.Websocket.Messages;
using Bitstamp.Client.Websocket.Utils;

namespace Bitstamp.Client.Websocket.Requests
{
	/// <summary>
	/// Base class for private requests
	/// </summary>
	public abstract class PrivateDataRequest : RequestBase
	{
		private readonly string _pair;
		private readonly Channel _channel;


		private string _authToken = null!;
		private int _userId;

		protected PrivateDataRequest(string pair, Channel channel)
		{
			_pair = pair;
			_channel = channel;
		}

		/// <summary>
		/// Sets the authentication token
		/// </summary>
		/// <param name="token">The token to use (expires after 60 seconds).</param>
		/// <param name="userId">The user Id from the token response.</param>
		public void SetAuthToken(string token, int userId)
		{
			_authToken = token;
			_userId = userId;
		}

		public PrivateRequestData Data => new PrivateRequestData
		{
			Channel = $"{ChannelType}_{CryptoPairsHelper.Clean(_pair)}-{_userId}",
			Auth = _authToken
		};

		private string ChannelType => _channel switch
		{
			Channel.Orders => ChannelPrefixes.PrivateMyOrders,
			Channel.Ticker => ChannelPrefixes.PrivateMyTrades,
			_ => throw new NotImplementedException()
		};
	}
}