using System.Runtime.Serialization;

namespace Bitstamp.Client.Websocket.Messages
{
	public enum EventType
	{
		[EnumMember(Value = ControlEventNames.Subscribe)]
		Subscribe,

		[EnumMember(Value = ControlEventNames.Unsubscribe)]
		Unsubscribe,

		[EnumMember(Value = ControlEventNames.Heartbeat)]
		Heartbeat,

		[EnumMember(Value = ControlEventNames.SubscriptionSucceeded)]
		SubscriptionSucceeded,

		[EnumMember(Value = ControlEventNames.UnsubscriptionSucceeded)]
		UnsubscriptionSucceeded,

		[EnumMember(Value = ControlEventNames.RequestReconnect)]
		RequestReconnect,

		[EnumMember(Value = ControlEventNames.Error)]
		Error,

		Trade,
		OrderCreated,
		OrderChanged,
		OrderDeleted,
		Data
	}
}