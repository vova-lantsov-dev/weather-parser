using System;

namespace WeatherParser.Utils
{
    static class UnitsConverter
    {
        public static double ConvertTemperatureKelvinToCelsius(double temperatureKelvin)
        {
            if (temperatureKelvin < 0)
            {
                throw new ArgumentException("Temperature in Kelvin cannot be less then 0");
            }

            return temperatureKelvin - 273.15;
        }
    }
}
