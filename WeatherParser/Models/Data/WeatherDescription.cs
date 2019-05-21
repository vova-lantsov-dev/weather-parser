using Newtonsoft.Json;

namespace WeatherParser.Models
{
    class WeatherDescription
    {
        [JsonProperty("main")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string FullDescription { get; set; }
    }
}
