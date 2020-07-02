using Newtonsoft.Json;

namespace Bitstamp.Client.Websocket.Requests
{
    public class RequestData
    {
        [JsonProperty("channel")] public string Channel { get; set; }
    }
}