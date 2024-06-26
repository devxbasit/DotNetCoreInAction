using System;
using System.Collections.Generic;
using System.Linq;
using ExceptionalHandling.ExceptionalHandling.Domain.Exception;

namespace ExceptionalHandling.Services
{
    public class WeatherService : IWeatherService
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public IEnumerable<WeatherForecast> Get(string cityName)
        {
            if (cityName.Length < 3)
            {
                throw new DomainValidationException("Invalid cityName");
            }

            if (cityName == "london")
            {
                throw new DomainNotFoundException("no data found");
            }


            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }
    }
}