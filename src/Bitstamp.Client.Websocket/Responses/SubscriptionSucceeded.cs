using System.Linq;
using System.Reactive.Subjects;
using Bitstamp.Client.Websocket.Json;
using Bitstamp.Client.Websocket.Messages;
using Newtonsoft.Json.Linq;

namespace Bitstamp.Client.Websocket.Responses
{
    public class SubscriptionSucceeded : ResponseBase
    {
        public override MessageType Event => MessageType.Subscribe;

        internal static bool TryHandle(JObject response, ISubject<SubscriptionSucceeded> subject)
        {
            var eventName = response?["event"];
            if (eventName == null || eventName.Value<string>() != "bts:subscription_succeeded") 
                return false;

            var parsed = response.ToObject<SubscriptionSucceeded>(BitstampJsonSerializer.Serializer);

            var channelName = response["channel"];
            if (parsed != null && channelName != null)
                parsed.Symbol = channelName.Value<string>().Split('_').LastOrDefault();

            subject.OnNext(parsed);
            return true;
        }
    }
}