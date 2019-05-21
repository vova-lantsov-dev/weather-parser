using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeatherParser.Models
{
    class OpenWeatherMapResponse
    {
        [JsonProperty("coord")]
        public Coordinates Coordinates { get; set; }

        [JsonProperty("weather")]
        public IEnumerable<WeatherDescription> WeatherDescription { get; set; }

        [JsonProperty("main")]
        public WeatherGeneralInformation WeatherGeneralInformation { get; set; }

        [JsonProperty("name")]
        public string AreaName { get; set; }
    }
}
