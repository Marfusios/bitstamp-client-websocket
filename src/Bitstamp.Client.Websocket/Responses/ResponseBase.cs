using Bitstamp.Client.Websocket.Messages;

namespace Bitstamp.Client.Websocket.Responses
{
	/// <summary>
	/// Message which is used as base for every response
	/// </summary>
	public abstract class ResponseBase : MessageBase
	{
		public string Channel { get; set; }
	}
}