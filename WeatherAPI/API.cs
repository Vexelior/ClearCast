using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAPI
{
    public class API
    {
        internal class WeatherInfo
        {
            [JsonProperty("name")]
            public string? City { get; set; }

            [JsonProperty("sys")]
            public LocationInfo? Location { get; set; }

            [JsonProperty("weather")]
            public WeatherDescription[]? Weather { get; set; }

            [JsonProperty("main")]
            public Main? WeatherDetails { get; set; }
        }

        public class WeatherDescription
        {
            [JsonProperty("description")]
            public string? Description { get; set; }

            [JsonProperty("icon")]
            public string? IconCode { get; set; }
        }

        public class Main
        {
            [JsonProperty("temp")]
            public double? Temperature { get; set; }

            [JsonProperty("humidity")]
            public int? Humidity { get; set; }
        }

        public class LocationInfo
        {
            [JsonProperty("city")]
            public string? City { get; set; }
        }
    }
}
