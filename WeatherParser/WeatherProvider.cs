using System;
using System.Configuration;
using System.Threading.Tasks;
using WeatherParser.Infrastructure;
using WeatherParser.Services;

namespace WeatherParser
{
    class WeatherProvider
    {
        private IWeatherParser WeatherParser { get; }
        private Logger _logger;

        public WeatherProvider()
        {
            WeatherParser = new OpenWeatherMapParser(
                ConfigurationManager.AppSettings["OpenWeatherMapApiUrl"],
                ConfigurationManager.AppSettings["OpenWeatherMapApiId"]);

            _logger = new Logger();
        }

        public async Task<string> GetWeatherForCity(string cityName)
        {
            try
            {
                _logger.WriteLog($"Begin getting weather for {cityName}");

                var result = await WeatherParser.Parse(cityName).ConfigureAwait(false);
                var formattedResult =
                    $"Current weather in {cityName}: {result.Title} ({result.Description}) - {result.TemperatureCelsius} C*, Pressure: {result.Pressure}";

                _logger.WriteLog($"Weather for {cityName} is gotten");

                return formattedResult;
            }
            catch (Exception e)
            {
                _logger.WriteExceptionLog(e);
            }

            return null;
        }
    }
}
