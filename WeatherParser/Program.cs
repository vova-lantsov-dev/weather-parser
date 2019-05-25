using System;
using System.Configuration;
using System.Net.Http;
using System.Threading;
using WeatherParser.Infrastructure;
using WeatherParser.Services;

namespace WeatherParser
{
    internal static class Program
    {
        private static readonly Logger _logger = new Logger();
        private static readonly HttpClient _client = new HttpClient();
        private static readonly string _apiUrl = ConfigurationManager.AppSettings["OpenWeatherMapApiUrl"];
        private static readonly string _forecastApiUrl = ConfigurationManager.AppSettings["OpenWeatherMapForecastApiUrl"];
        private static readonly string _apiKey = ConfigurationManager.AppSettings["OpenWeatherMapApiId"];

        private static void Main(string[] args)
        {
            _logger.WriteLog("Application started");

            var cancellationSource = new CancellationTokenSource();

            new WeatherProvider(_logger, new NewOpenWeatherMapParser(_logger, _client, _forecastApiUrl, _apiKey))
            .GetWeatherForCityAsync("London", cancellationSource.Token)
            .ContinueWith(task =>
            {
                if (task.IsFaulted)
                    _logger.WriteExceptionLog(task.Exception);
                else if (!task.IsCanceled)
                    _logger.WriteLog(task.Result);
            });

            new WeatherProvider(_logger, new OpenWeatherMapParser(_logger, _client, _apiUrl, _apiKey))
            .GetWeatherForCityAsync("London", cancellationSource.Token)
            .ContinueWith(task =>
            {
                if (task.IsFaulted)
                    _logger.WriteExceptionLog(task.Exception);
                else if (!task.IsCanceled)
                    _logger.WriteLog(task.Result);
            });

            Console.ReadKey();
            cancellationSource.Cancel();
            _logger.WriteLog("Application shut down");
            Thread.Sleep(3000);

            _client.Dispose();
        }
    }
}
