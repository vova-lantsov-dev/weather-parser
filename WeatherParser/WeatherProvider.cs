using System;
using System.Configuration;
using System.Threading.Tasks;
using WeatherParser.Infrastructure;
using WeatherParser.Services;

namespace WeatherParser
{
    internal sealed class WeatherProvider
    {
        private IWeatherParser WeatherParser { get; }
        private readonly Logger _logger = new Logger();

        public WeatherProvider()
        {
            WeatherParser = new OpenWeatherMapParser(
                ConfigurationManager.AppSettings["OpenWeatherMapApiUrl"],
                ConfigurationManager.AppSettings["OpenWeatherMapApiId"]);
        }

        public async Task<string> GetWeatherForCity(string cityName)
        {
            try
            {
                _logger.WriteLog($"Begin getting weather for {cityName}");

                var result = await WeatherParser.Parse(cityName).ConfigureAwait(false);
                var formattedResult = result == null ?
                    "Result equals to null." :
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
