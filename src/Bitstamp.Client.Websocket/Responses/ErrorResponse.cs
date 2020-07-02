using System.Reactive.Subjects;
using Bitstamp.Client.Websocket.Json;
using Bitstamp.Client.Websocket.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bitstamp.Client.Websocket.Responses
{
    /// <summary>
    /// Error messages: Most failure cases will cause an error message (a message with the type "error") to be emitted.
    /// This can be helpful for implementing a client or debugging issues.
    /// </summary>
    public class ErrorResponse : ResponseBase
    {
        public override MessageType Event => MessageType.Error;

        [JsonProperty("code")] public object Code { get; set; }

        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        internal static bool TryHandle(JObject response, ISubject<ErrorResponse> subject)
        {
            if (response?["event"].Value<string>() != "bts:error") return false;

            var parsed = response.ToObject<ErrorResponse>(BitstampJsonSerializer.Serializer);
            subject.OnNext(parsed);
            return true;
        }
    }
}