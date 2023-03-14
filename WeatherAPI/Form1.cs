using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using static WeatherAPI.API;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;

namespace WeatherAPI
{
    public partial class Form1 : Form
    {
        public string? UserCity { get; set; }

        private const string OpenWeatherMapApiKey = "49c8545070487501c919486dbf8afdaf";
        private const string OpenWeatherMapApiUrl = "https://api.openweathermap.org/data/2.5/weather";

        public Form1()
        {
            InitializeComponent();
            // Display the user's weather based on their geolocation. \\
            SearchByLocation();
        }

        private async void FindWeatherDetails(string city, string region, string country)
        {
            // Create a list to store the city, region and country. \\
            List<string> locationDetails = new();
            locationDetails.Add(city);
            locationDetails.Add(region);
            locationDetails.Add(country);

            using HttpClient client = new();
            UriBuilder builder = new(OpenWeatherMapApiUrl);
            builder.Query = $"q={city},{region},{country}&appid={OpenWeatherMapApiKey}&units=metric";
            HttpResponseMessage response = await client.GetAsync(builder.ToString());

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    string json = await response.Content.ReadAsStringAsync();
                    WeatherInfo? info = JsonConvert.DeserializeObject<WeatherInfo>(json);

                    // Display the region and country. \\
                    if (locationDetails[1] != null && locationDetails[2] != null)
                    {
                        cityLabel.Text = $"{locationDetails[0]}, {locationDetails[1]}, {locationDetails[2]}";
                    }
                    else
                    {
                        cityLabel.Text = $"{locationDetails[0]}, null, null";
                    }

                    temperatureLabel.Text = $"{info?.WeatherDetails?.Temperature}°C";
                    descriptionLabel.Text = info?.Weather?[0].Description;
                    humidityLabel.Text = $"{info?.WeatherDetails?.Humidity}%";

                    // Celcius to Fahrenheit conversion. \\
                    if (temperatureLabel.Text != null)
                    {
                        double temp = Convert.ToDouble(temperatureLabel.Text[0..^2]);
                        double fahrenheit = (temp * 9 / 5) + 32;
                        fahrenheit = Math.Round(fahrenheit, 1);
                        temperatureLabel.Text = $"{fahrenheit}°F";
                    }

                    // Make the first character of the description uppercase, and new words start with uppercase after a space. \\
                    if (descriptionLabel.Text != null)
                    {
                        string description = descriptionLabel.Text;
                        description = description.Substring(0, 1).ToUpper() + description[1..];
                        description = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(description);
                        descriptionLabel.Text = description;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Error retrieving weather information.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static async Task<List<string>> GetUserLocation()
        {
            List<string> locationInfo = new();
            using HttpClient client = new();
            UriBuilder builder = new("https://ipinfo.io/json");
            HttpResponseMessage response = await client.GetAsync(builder.ToString());
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    string json = await response.Content.ReadAsStringAsync();
                    LocationInfo? info = JsonConvert.DeserializeObject<LocationInfo>(json);
                    // Search through the json file to find the city. \\
                    JObject jsonQuery = JObject.Parse(json);
                    string city = jsonQuery["city"].ToString();
                    string region = jsonQuery["region"].ToString();
                    string country = jsonQuery["country"].ToString();

                    locationInfo.Add(city);
                    locationInfo.Add(region);
                    locationInfo.Add(country);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Error retrieving location information.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return locationInfo;
        }

        private async void SearchByLocation()
        {
            List<string> city = await GetUserLocation();

            if (city != null)
            {
                // Find possible region and country for the city entered. \\
                FindWeatherDetails(city[0], city[1], city[2]);
            }
            else
            {
                MessageBox.Show("Please enter a city.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void Search(object sender, EventArgs e)
        {
            string city = cityTextBox.Text;

            if (!string.IsNullOrEmpty(city))
            {
                List<string> locationDetails = await LocationDetails(city);
                FindWeatherDetails(locationDetails[0], locationDetails[1], locationDetails[2]);
            }
            else 
            {
                MessageBox.Show("Please enter a city.", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static async Task<List<string>> LocationDetails(string city)
        {
            List<string> locationDetails = new();

            using HttpClient client = new();
            UriBuilder builder = new(OpenWeatherMapApiUrl);
            builder.Query = $"q={city}&appid={OpenWeatherMapApiKey}&units=metric";
            HttpResponseMessage response = client.GetAsync(builder.ToString()).Result;

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    // Get the longitude and latitude of the city entered. \\
                    string json = await response.Content.ReadAsStringAsync();
                    JObject jsonQuery = JObject.Parse(json);
                    string longitude = jsonQuery["coord"]["lon"].ToString();
                    string latitude = jsonQuery["coord"]["lat"].ToString();

                    // From the longitude and latitude, find the region and country. \\
                    UriBuilder builder2 = new("https://api.bigdatacloud.net/data/reverse-geocode-client");
                    builder2.Query = $"latitude={latitude}&longitude={longitude}&localityLanguage=en";
                    HttpResponseMessage response2 = client.GetAsync(builder2.ToString()).Result;

                    if (response2.IsSuccessStatusCode)
                    {
                        string json2 = await response2.Content.ReadAsStringAsync();
                        JObject jsonQuery2 = JObject.Parse(json2);
                        city = jsonQuery2["city"].ToString();
                        string region = jsonQuery2["principalSubdivision"].ToString();
                        string country = jsonQuery2["continentCode"].ToString();

                        if (country == "NA")
                        {
                            country = "US";
                        }

                        locationDetails.Add(city);
                        locationDetails.Add(region);
                        locationDetails.Add(country);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Error retrieving weather information.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return locationDetails;
        }
    }
}