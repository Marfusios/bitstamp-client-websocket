using System;

namespace Bitstamp.Client.Websocket.Utils
{
    internal static class UnixTime
    {
        public static readonly DateTime UnixBase = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long NowMs()
        {
            var subtracted = DateTime.UtcNow.Subtract(UnixBase);
            return (long) subtracted.TotalMilliseconds;
        }

        public static long NowTicks()
        {
            return DateTime.UtcNow.Ticks - UnixBase.Ticks;
        }

        public static DateTime ConvertToTimeFromMilliseconds(long timeInMs)
        {
            return UnixBase.AddMilliseconds(timeInMs);
        }

        public static DateTime? ConvertToTimeFromMilliseconds(long? timeInMs)
        {
            if (!timeInMs.HasValue)
                return null;
            return UnixBase.AddMilliseconds(timeInMs.Value);
        }

        public static DateTime ConvertToTimeFromSeconds(long timeInSec)
        {
            return UnixBase.AddSeconds(timeInSec);
        }

        public static DateTime? ConvertToTimeFromSeconds(long? timeInMs)
        {
            if (!timeInMs.HasValue)
                return null;
            return UnixBase.AddSeconds(timeInMs.Value);
        }
    }
}