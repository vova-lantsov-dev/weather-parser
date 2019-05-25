using System.Threading;
using System.Threading.Tasks;
using WeatherParser.Models;

namespace WeatherParser.Services
{
    interface IWeatherParser
    {
        Task<WeatherResult> ParseAsync(string cityName, CancellationToken cancellationToken);
    }
}
