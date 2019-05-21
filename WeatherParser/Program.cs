using System;
using System.Threading.Tasks;
using WeatherParser.Infrastructure;

namespace WeatherParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var _logger = new Logger();
            _logger.WriteLog("Application started");

            var weatherProvider = new WeatherProvider();
            var currentWeather = weatherProvider.GetWeatherForCity("London").GetAwaiter().GetResult();
            Console.Write(currentWeather);

            Console.ReadKey();
            _logger.WriteLog("Application shut down");
        }
    }
}
