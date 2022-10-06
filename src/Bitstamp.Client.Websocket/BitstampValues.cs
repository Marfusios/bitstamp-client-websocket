using System;

namespace Bitstamp.Client.Websocket;

/// <summary>
/// Bitstamp Pro static urls
/// </summary>
public static class BitstampValues
{
    /// <summary>
    /// Main Bitstamp url to websocket API
    /// </summary>
    public static readonly Uri ApiWebsocketUrl = new("wss://ws.bitstamp.net");
}