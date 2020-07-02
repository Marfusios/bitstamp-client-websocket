using System.Linq;
using System.Reactive.Subjects;
using Bitstamp.Client.Websocket.Json;
using Bitstamp.Client.Websocket.Messages;
using Newtonsoft.Json.Linq;

namespace Bitstamp.Client.Websocket.Responses
{
    public class UnsubscriptionSucceeded : ResponseBase
    {
        public override MessageType Event => MessageType.Unsubscribe;

        internal static bool TryHandle(JObject response, ISubject<UnsubscriptionSucceeded> subject)
        {
            var eventName = response?["event"];
            if (eventName == null || eventName.Value<string>() != "bts:unsubscription_succeeded") 
                return false;

            var parsed = response.ToObject<UnsubscriptionSucceeded>(BitstampJsonSerializer.Serializer);

            var channelName = response["channel"];
            if (parsed != null && channelName != null)
                parsed.Symbol = channelName.Value<string>().Split('_').LastOrDefault();

            subject.OnNext(parsed);
            return true;
        }
    }
}