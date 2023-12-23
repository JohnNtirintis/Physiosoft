using NLog;
using System.Runtime.CompilerServices;
using ILogger = NLog.ILogger;

namespace Physiosoft.Logger

{
    public class NLogger 
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private static string? logMessage;

        public static void LogInfo(string message, [CallerMemberName] string memberName = "")
        {
            logMessage = $"{memberName}: {message}";
            logger.Info(logMessage);
        }

        public static void LogWarn(string message, [CallerMemberName] string memberName = "")
        {
            logMessage = $"{memberName}: {message}";
            logger.Warn(logMessage);
        }

        public static void LogError(string message, [CallerMemberName] string memberName = "")
        {
            logMessage = $"{memberName}: {message}";
            logger.Error(logMessage);
        }

        public static 
            void LogError(Exception ex, string message, [CallerMemberName] string memberName = "")
        {
            logMessage = $"{memberName}: {message}";
            logger.Error(logMessage);
        }

        public static void LogDebug(string message, [CallerMemberName] string memberName = "")
        {
            logMessage = $"{memberName}: {message}";
            logger.Debug(logMessage);
        }
    }
}
