using System.Collections.Generic;

namespace ExceptionalHandling.Services
{
    public interface IWeatherService
    {
        IEnumerable<WeatherForecast> Get(string cityName);
    }
}