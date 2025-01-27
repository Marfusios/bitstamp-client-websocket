namespace Bitstamp.Client.Websocket.Requests
{
	public class RequestData
	{
		public string Channel { get; set; }
	}

	public class PrivateRequestData : RequestData
	{
		public string Auth { get; set; }
	}
}