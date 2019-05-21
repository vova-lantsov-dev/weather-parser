using System;
using System.Threading.Tasks;
using WeatherParser.Infrastructure;

namespace WeatherParser
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var _logger = new Logger();
            _logger.WriteLog("Application started");

            var weatherProvider = new WeatherProvider();
            var currentWeather = await weatherProvider.GetWeatherForCity("Angola");
            Console.Write(currentWeather);

            Console.ReadKey();
            _logger.WriteLog("Application shut down");
            await Task.Delay(3000);
        }
    }
}
