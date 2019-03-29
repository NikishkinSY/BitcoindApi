using Microsoft.Extensions.Logging;

namespace Bitcoind.Service.Helpers
{
    public static class LoggerHelper
    {
        public static void SetUpLoggerFactory(ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/{Date}.txt", LogLevel.Error);
        }
    }
}
