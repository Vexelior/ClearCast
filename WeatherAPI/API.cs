using Newtonsoft.Json;

namespace WeatherAPI
{
    public class API
    {
        internal class WeatherInfo
        {
            [JsonProperty("name")]
            public string City { get; set; }

            [JsonProperty("sys")]
            public LocationInfo Location { get; set; }

            [JsonProperty("weather")]
            public WeatherDescription[] Weather { get; set; }

            [JsonProperty("main")]
            public Main WeatherDetails { get; set; }
        }

        public class WeatherDescription
        {
            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("icon")]
            public string IconCode { get; set; }
        }

        public class Main
        {
            [JsonProperty("temp")]
            public double Temperature { get; set; }

            [JsonProperty("humidity")]
            public int Humidity { get; set; }
        }

        public class LocationInfo
        {
            [JsonProperty("city")]
            public string City { get; set; }

            [JsonProperty("region")]
            public string Region { get; set; }

            [JsonProperty("country")]
            public string Country { get; set; }

            [JsonProperty("post code")]
            public string Zipcode { get; set; }
        }
    }
}
