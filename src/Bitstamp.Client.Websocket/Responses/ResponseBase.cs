using System;
using Bitstamp.Client.Websocket.Messages;
using Newtonsoft.Json;

namespace Bitstamp.Client.Websocket.Responses
{
    /// <summary>
    /// Message which is used as base for every request and response
    /// </summary>
    public abstract class ResponseBase : MessageBase
    {
        public string Channel { get; set; }
        public string Symbol { get; set; }
    }
}