using NLog;
using ILogger = NLog.ILogger;

namespace Physiosoft.Logger

{
    public class NLogger 
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public static void LogInfo(string message)
        {
            logger.Info(message);
        }

        public static void LogWarn(string message)
        {
            logger.Warn(message);
        }

        public static void LogError(string message)
        {
            logger.Error(message);
        }

        public static void LogError(Exception ex, string message)
        {
            logger.Error(ex, message);
        }

        public static void LogDebug(string message)
        {
            logger.Debug(message);
        }
    }
}
