using System;
using System.Threading;
using System.Threading.Tasks;
using WeatherParser.Infrastructure;
using WeatherParser.Services;

namespace WeatherParser
{
    internal sealed class WeatherProvider
    {
        private readonly IWeatherParser _weatherParser;
        private readonly Logger _logger;

        public WeatherProvider(Logger logger, IWeatherParser instance)
        {
            _weatherParser = instance;
            _logger = logger;
        }

        public async Task<string> GetWeatherForCityAsync(string cityName, CancellationToken cancellationToken)
        {
            _logger.WriteLog($"Begin getting weather for {cityName}");

            var result = await _weatherParser.ParseAsync(cityName, cancellationToken).ConfigureAwait(false) ?? throw new ArgumentException("Result equals to null.", "result");
            var formattedResult =
                $"Current weather in {cityName}: {result.Title} ({result.Description}) - {result.TemperatureCelsius} C*, Pressure: {result.Pressure}";

            _logger.WriteLog($"Weather for {cityName} is gotten");

            return formattedResult;
        }
    }
}
