using System;
using System.Configuration;
using System.Threading.Tasks;
using WeatherParser.Infrastructure;
using WeatherParser.Services;

namespace WeatherParser
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var _logger = new Logger();
            _logger.WriteLog("Application started");

            var weatherProvider = new WeatherProvider(new NewOpenWeatherMapParser());
            var currentWeather = await weatherProvider.GetWeatherForCityAsync("London");
            weatherProvider = new WeatherProvider(new OpenWeatherMapParser(
                ConfigurationManager.AppSettings["OpenWeatherMapApiUrl"],
                ConfigurationManager.AppSettings["OpenWeatherMapApiId"]));
            currentWeather = await weatherProvider.GetWeatherForCityAsync("London");
            Console.Write(currentWeather);

            Console.ReadKey();
            _logger.WriteLog("Application shut down");
            await Task.Delay(3000);
        }
    }
}
