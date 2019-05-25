using System;
using System.Threading.Tasks;
using WeatherParser.Infrastructure;
using WeatherParser.Services;

namespace WeatherParser
{
    internal sealed class WeatherProvider
    {
        private readonly IWeatherParser _weatherParser;
        private readonly Logger _logger = new Logger();

        public WeatherProvider(IWeatherParser instance)
        {
            _weatherParser = instance;
        }

        public async Task<string> GetWeatherForCityAsync(string cityName)
        {
            try
            {
                _logger.WriteLog($"Begin getting weather for {cityName}");

                var result = await _weatherParser.Parse(cityName).ConfigureAwait(false) ?? throw new ArgumentException("Result equals to null.", "result");
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
