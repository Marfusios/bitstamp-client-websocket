using Newtonsoft.Json;

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
        [JsonProperty("event")]
        public virtual string Event { get; set; }

        [JsonProperty("data")] public virtual RequestData RequestData { get; set; }
    }
}