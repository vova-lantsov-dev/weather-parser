using System;
using System.Configuration;
using System.Threading;
using WeatherParser.Infrastructure;
using WeatherParser.Services;

namespace WeatherParser
{
    internal static class Program
    {
        private static readonly Logger _logger = new Logger();
        private static readonly string _apiUrl = ConfigurationManager.AppSettings["OpenWeatherMapApiUrl"];
        private static readonly string _apiKey = ConfigurationManager.AppSettings["OpenWeatherMapApiId"];

        private static void Main(string[] args)
        {
            _logger.WriteLog("Application started");

            var tasks = new[]
            {
                new WeatherProvider(new NewOpenWeatherMapParser())
                .GetWeatherForCityAsync("London")
                .ContinueWith(task => _logger.WriteLog(task.Result)),

                new WeatherProvider(new OpenWeatherMapParser(_apiUrl, _apiKey))
                .GetWeatherForCityAsync("London")
                .ContinueWith(task => _logger.WriteLog(task.Result))
            };

            Console.ReadKey();
            _logger.WriteLog("Application shut down");
            Thread.Sleep(3000);
        }
    }
}
