namespace Bitstamp.Client.Websocket.Requests
{
	/// <summary>
	/// Base class for every request
	/// </summary>
	public abstract class RequestBase
	{
		/// <summary>
		/// Unique request type
		/// </summary>
		public abstract string Event { get; }
	}
}