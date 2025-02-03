namespace Bitstamp.Client.Websocket.Messages
{
	internal static class ControlEventNames
	{
		public const string Subscribe = "bts:subscribe";
		public const string Unsubscribe = "bts:unsubscribe";
		public const string Heartbeat = "bts:heartbeat";
		public const string SubscriptionSucceeded = "bts:subscription_succeeded";
		public const string UnsubscriptionSucceeded = "bts:unsubscription_succeeded";
		public const string RequestReconnect = "bts:request_reconnect";
		public const string Error = "bts:error";
	}
}