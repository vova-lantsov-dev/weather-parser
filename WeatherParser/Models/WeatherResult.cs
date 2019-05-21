namespace WeatherParser.Models
{
    class WeatherResult
    {
        public WeatherResult(string title, string description, int temperatureCelsius, int pressure)
        {
            Title = title;
            Description = description;
            TemperatureCelsius = temperatureCelsius;
            Pressure = pressure;
        }

        public string Title { get; }

        public string Description { get; }

        public int TemperatureCelsius { get; }

        public int Pressure { get; }
    }
}
