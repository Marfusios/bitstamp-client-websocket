namespace Bitstamp.Client.Websocket.Utils
{
    /// <summary>
    /// Helper class for working with pair identifications
    /// </summary>
    internal static class CryptoPairsHelper
    {
        /// <summary>
        /// Clean pair from any unnecessary characters and make lowercase
        /// </summary>
        public static string Clean(string pair)
        {
            return (pair ?? string.Empty)
                .Trim()
                .ToLower()
                .Replace("/", "")
                .Replace("-", "")
                .Replace("\\", "");
        }
    }
}