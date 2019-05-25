using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using WeatherParser.Infrastructure;
using WeatherParser.Models;
using WeatherParser.Utils;

namespace WeatherParser.Services
{
    internal sealed class OpenWeatherMapParser : IWeatherParser
    {
        private readonly string _apiUrl;
        private readonly string _apiKey;

        private readonly Logger _logger;
        private readonly HttpClient _client;

        public OpenWeatherMapParser(Logger logger, HttpClient client, string apiUrl, string apiKey)
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

            var parsedResponse = JsonConvert.DeserializeObject<OpenWeatherMapResponse>(responseJson);

            return MapResponseToWeatherResult(parsedResponse);
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

        private WeatherResult MapResponseToWeatherResult(OpenWeatherMapResponse response)
        {
            return response == null ? null : new WeatherResult(
                response.WeatherDescription?.FirstOrDefault()?.Title,
                response.WeatherDescription?.FirstOrDefault()?.FullDescription,
                (int)Math.Round(UnitsConverter.ConvertTemperatureKelvinToCelsius((double)(response.WeatherGeneralInformation?.Temperature ?? 0M)), 0),
                response.WeatherGeneralInformation?.Pressure ?? 0
            );
        }
    }
}
