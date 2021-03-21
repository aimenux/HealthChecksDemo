using System;

namespace WebApi.Models
{
    public class WeatherDetails
    {
        public DateTime Date { get; set; }

        public int Temperature { get; set; }

        public string Summary { get; set; }
    }
}
