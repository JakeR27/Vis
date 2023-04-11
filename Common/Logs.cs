using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.Common
{
    public static class Logs
    {
        private static string[] _logLevelStrings = { "DBG", "INF", "WRN", "ERR" };
        private static LogLevel _logLevel = LogLevel.Error;

        public static LogLevel LogLevelFromInt(int level)
        {
            return (LogLevel)level;
        }
        
        public enum LogLevel
        {
            Debug = 0,
            Info = 1,
            Warning = 2,
            Error = 3,
        }

        public static void LogDebug(string message) => Log(LogLevel.Debug, message);
        public static void LogInfo(string message) => Log(LogLevel.Info, message);
        public static void LogWarning(string message) => Log(LogLevel.Warning, message);
        public static void LogError(string message) => Log(LogLevel.Error, message);

        public static void Log(LogLevel level, string message)
        {
            //return;
            if (level < _logLevel) return;

            string time = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Console.WriteLine(
                "[{0}][{1}] {2}", 
                time, 
                _logLevelStrings[(int)level], 
                message
            );
        }

        public static void SetLogLevel(LogLevel newLogLevel)
        {
            _logLevel = newLogLevel;
        }
        public static void SetLogLevel(int newLogLevel)
        {
            _logLevel = LogLevelFromInt(newLogLevel);
        }
    }
}
