using System.Threading.Tasks;
using WeatherParser.Models;

namespace WeatherParser.Services
{
    interface IWeatherParser
    {
        Task<WeatherResult> Parse(string cityName);
    }
}
