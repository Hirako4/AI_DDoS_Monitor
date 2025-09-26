using NLog;

namespace AI_DDoS_Monitor.Logging
{
    public static class Logger
    {
        private static readonly NLog.Logger log = LogManager.GetCurrentClassLogger();

        public static void Info(string message) => log.Info(message);
        public static void Error(string message) => log.Error(message);
    }
}