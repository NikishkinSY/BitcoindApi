using System;

namespace Bitcoind.Core.Helpers
{
    public static class IntExtension
    {
        public static DateTime FromUnixToDateTime(this int seconds)
        {
            var baseDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            return baseDateTime.AddSeconds(seconds);
        }
    }
}
