using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
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

        private readonly Logger _logger = new Logger();

        public OpenWeatherMapParser(string apiUrl, string apiKey)
        {
            _apiUrl = apiUrl;
            _apiKey = apiKey;
        }

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
                    string apiRequestUrl = GetApiRequestUrl(cityName);

                    _logger.WriteLog("Begin request to OpenWeatherMap Api");
                    var httpResponse = await httpClient.GetAsync(apiRequestUrl);
                    _logger.WriteLog($"Response from OpenWeatherMap Api received. Http Code: {httpResponse?.StatusCode}");

                    var responseJson = await httpResponse.Content.ReadAsStringAsync();
                    _logger.WriteLog($"Response from OpenWeatherMap: {responseJson}");

                    var parsedResponse = JsonConvert.DeserializeObject<OpenWeatherMapResponse>(responseJson);

                    return MapResponseToWeatherResult(parsedResponse);
                }
            }
            catch (Exception e)
            {
                _logger.WriteExceptionLog(e);
            }

            return null;
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
