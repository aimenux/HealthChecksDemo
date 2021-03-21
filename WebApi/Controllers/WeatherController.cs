using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Logging;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly ILogger<WeatherController> _logger;

        private static readonly Random Random = new(Guid.NewGuid().GetHashCode());

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherController(ILogger<WeatherController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public WeatherDetails GetWeatherDetails()
        {
            var details = new WeatherDetails
            {
                Temperature = Random.Next(-20, 55),
                Date = DateTime.Now.AddDays(Random.Next(1, 10)),
                Summary = Summaries[Random.Next(Summaries.Length)]
            };

            _logger.LogTrace("Weather details: {@details}", details);

            return details;
        }
    }
}
