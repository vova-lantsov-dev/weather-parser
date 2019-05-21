using System;
using System.IO;
using System.Reflection;

namespace WeatherParser.Infrastructure
{
    public class Logger
    {
        private const string LogFileName = "log.txt";
        private readonly string _logFilePath;

        public Logger()
        {
            _logFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\" + LogFileName;
        }

        public void WriteLog(string message)
        {
            try
            {
                var formattedMessage = $"> {DateTime.Now.ToString()} > {message}{Environment.NewLine}{Environment.NewLine}";
                File.AppendAllText(_logFilePath, formattedMessage);
            }
            catch
            {
                // silent behavior is preferred
            }
        }

        public void WriteExceptionLog(Exception exception)
        {
            var logMessage = $"{exception.Message}; {exception.InnerException?.Message}{Environment.NewLine}{exception.StackTrace}";
            WriteLog(logMessage);
        }
    }
}
