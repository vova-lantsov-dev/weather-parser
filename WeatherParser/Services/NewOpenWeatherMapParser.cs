using Newtonsoft.Json;
using System;
using System.Dynamic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using WeatherParser.Infrastructure;
using WeatherParser.Models;
using WeatherParser.Utils;

namespace WeatherParser.Services
{
    internal sealed class NewOpenWeatherMapParser : IWeatherParser
    {
        private readonly Logger _logger = new Logger();
        private readonly HttpClient _client;

        private readonly string _apiKey;
        private readonly string _apiUrl;

        public NewOpenWeatherMapParser(Logger logger, HttpClient client, string apiUrl, string apiKey)
        {
            _logger = logger;
            _apiUrl = apiUrl;
            _apiKey = apiKey;
            _client = client;
        }

        public async Task<WeatherResult> ParseAsync(string cityName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(cityName))
            {
                throw new ArgumentException("cityName is required");
            }

            cancellationToken.ThrowIfCancellationRequested();

            string apiRequestUrl = GetApiRequestUrl(cityName);

            _logger.WriteLog("Begin request to OpenWeatherMap Api");
            var httpResponse = await _client.GetAsync(apiRequestUrl, cancellationToken);
            _logger.WriteLog($"Response from OpenWeatherMap Api received. Http Code: {httpResponse?.StatusCode}");

            cancellationToken.ThrowIfCancellationRequested();

            var responseJson = await httpResponse.Content.ReadAsStringAsync();
            _logger.WriteLog($"Response from OpenWeatherMap: {responseJson}");

            cancellationToken.ThrowIfCancellationRequested();

            dynamic parsedResponse = JsonConvert.DeserializeObject<ExpandoObject>(responseJson);
            var last = parsedResponse.list.Count > 0 ? parsedResponse.list[parsedResponse.list.Count - 1] : null;
            if (last == null)
                return null;

            var lastWeather = (last.weather?.Count ?? 0) >= 1 ? last.weather[0] : null;
            return new WeatherResult((string)lastWeather?.main, (string)lastWeather?.description,
                (int)Math.Round(UnitsConverter.ConvertTemperatureKelvinToCelsius((double)(last.main?.temp ?? 0M)), 0),
                (int?)last.main?.pressure ?? 0);
        }

        private string GetApiRequestUrl(string cityName)
        {
            var builder = new UriBuilder(_apiUrl);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["appid"] = _apiKey;
            query["q"] = cityName;
            builder.Query = query.ToString();
            return builder.ToString();
        }
    }
}
