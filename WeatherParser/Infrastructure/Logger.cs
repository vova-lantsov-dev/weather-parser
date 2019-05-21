using System;
using System.IO;
using System.Reflection;

namespace WeatherParser.Infrastructure
{
    public sealed class Logger
    {
        private const string LogFileName = "log.txt";
        private readonly string _logFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), LogFileName);

        private static string Line => Environment.NewLine;

        public void WriteLog(string message)
        {
            try
            {
                var formattedMessage = $"> {DateTime.Now:d.MM.yyyy h:mm:ss} > {message}{Line}{Line}";
                File.AppendAllText(_logFilePath, formattedMessage);
            }
            catch
            {
                // silent behavior is preferred
            }

            Console.WriteLine(message);
        }

        public void WriteExceptionLog(Exception exception)
        {
            var logMessage = $"{exception.Message}; {exception.InnerException?.Message}{Line}{exception.StackTrace}";
            WriteLog(logMessage);
        }
    }
}
