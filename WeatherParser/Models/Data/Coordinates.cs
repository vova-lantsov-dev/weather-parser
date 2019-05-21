using Newtonsoft.Json;

namespace WeatherParser.Models
{
    class Coordinates
    {
        [JsonProperty("lon")]
        public decimal Longitude { get; set; }

        [JsonProperty("lat")]
        public decimal Latitude { get; set; }
    }
}
