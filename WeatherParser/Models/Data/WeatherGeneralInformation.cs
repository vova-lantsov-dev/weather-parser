using Newtonsoft.Json;

namespace WeatherParser.Models
{
    class WeatherGeneralInformation
    {
        [JsonProperty("temp")]
        public decimal Temperature { get; set; }

        [JsonProperty("pressure")]
        public int Pressure { get; set; }

        [JsonProperty("humidity")]
        public int Humidity { get; set; }
    }
}
