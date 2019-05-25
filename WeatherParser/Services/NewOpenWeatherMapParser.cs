using Newtonsoft.Json;
using System;
using System.Dynamic;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherParser.Infrastructure;
using WeatherParser.Models;
using WeatherParser.Utils;

namespace WeatherParser.Services
{
    internal sealed class NewOpenWeatherMapParser : IWeatherParser
    {
        private readonly Logger _logger = new Logger();

        public async Task<WeatherResult> Parse(string cityName)
        {
            if (string.IsNullOrEmpty(cityName))
            {
                throw new ArgumentException("cityName is required");
            }

            try
            {
                using (var httpClient = new HttpClient())
                {
                    _logger.WriteLog("Begin request to OpenWeatherMap Api");
                    var httpResponse = await httpClient.GetAsync("https://samples.openweathermap.org/data/2.5/forecast/hourly?q=London,us&appid=b6907d289e10d714a6e88b30761fae22");
                    _logger.WriteLog($"Response from OpenWeatherMap Api received. Http Code: {httpResponse?.StatusCode}");

                    var responseJson = await httpResponse.Content.ReadAsStringAsync();
                    _logger.WriteLog($"Response from OpenWeatherMap: {responseJson}");

                    dynamic parsedResponse = JsonConvert.DeserializeObject<ExpandoObject>(responseJson);
                    var last = parsedResponse.list.Count > 0 ? parsedResponse.list[parsedResponse.list.Count - 1] : null;
                    if (last == null)
                        return null;

                    var lastWeather = (last.weather?.Count ?? 0) >= 1 ? last.weather[0] : null;
                    return new WeatherResult((string)lastWeather?.main, (string)lastWeather?.description,
                        (int)Math.Round(UnitsConverter.ConvertTemperatureKelvinToCelsius((double)(last.main?.temp ?? 0M)), 0),
                        (int?)last.main?.pressure ?? 0);
                }
            }
            catch (Exception e)
            {
                _logger.WriteExceptionLog(e);
            }

            return null;
        }
    }
}
