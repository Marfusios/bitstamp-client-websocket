namespace Bitstamp.Client.Websocket
{
    public enum Event
    {
        Undefined,
        Trade,
        OrderCreated,
        OrderChanged,
        OrderDeleted,
        Data,
        Error,
        SubscriptionSucceeded,
        Subscribe,
        Order
    }
}