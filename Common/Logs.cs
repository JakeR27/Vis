using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vis.Common
{
    public static class Logs
    {
        private static string[] _logLevelStrings = { "DBG", "ERR", "WRN", "INF" };

        public enum LogLevel
        {
            Debug = 0,
            Error = 1,
            Warning = 2,
            Info = 3,
        }

        public static void Log(LogLevel level, string message)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Console.WriteLine(
                "[{0}][{1}] {2}", 
                time, 
                _logLevelStrings[(int)level], 
                message
            );
        }
    }
}
