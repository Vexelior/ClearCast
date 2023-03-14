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
        private const string OpenWeatherMapApiKey = "49c8545070487501c919486dbf8afdaf";
        private const string OpenWeatherMapApiUrl = "https://api.openweathermap.org/data/2.5/weather";
        public string? UserCity { get; set; }

        public Form1()
        {
            InitializeComponent();
            SearchByLocation();
        }

        private async void FindWeatherDetails(string city)
        {
            using HttpClient client = new();
            UriBuilder builder = new(OpenWeatherMapApiUrl);
            builder.Query = $"q={city}&appid={OpenWeatherMapApiKey}&units=metric";
            HttpResponseMessage response = await client.GetAsync(builder.ToString());

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    string json = await response.Content.ReadAsStringAsync();
                    WeatherInfo? info = JsonConvert.DeserializeObject<WeatherInfo>(json);

                    JObject jsonQuery = JObject.Parse(json);
                    string country = jsonQuery["sys"]["country"].ToString();
                    
                    // if the country is the united states, display the state as well
                    if (country == "US")
                    {
                        // Using the coordinates, find the state
                        string lat = jsonQuery["coord"]["lat"].ToString();
                        string lon = jsonQuery["coord"]["lon"].ToString();
                        UriBuilder builder2 = new("https://api.bigdatacloud.net/data/reverse-geocode-client");
                        builder2.Query = $"latitude={lat}&longitude={lon}&localityLanguage=en";
                        HttpResponseMessage response2 = await client.GetAsync(builder2.ToString());
                        string json2 = await response2.Content.ReadAsStringAsync();
                        JObject jsonQuery2 = JObject.Parse(json2);
                        string state = jsonQuery2["principalSubdivision"].ToString();
                        country = $"{state}, {country}";
                    }
                    else if (string.IsNullOrEmpty(country))
                    {
                        country = "";
                    }

                    if (jsonQuery == null)
                    {
                        country = "";
                    }

                    cityLabel.Text = $"{info?.City}, {country}";
                    temperatureLabel.Text = $"{info?.WeatherDetails?.Temperature}°C";
                    descriptionLabel.Text = info?.Weather?[0].Description;
                    humidityLabel.Text = $"{info?.WeatherDetails?.Humidity}%";

                    // Celcius to Fahrenheit
                    if (temperatureLabel.Text != null)
                    {
                        double temp = Convert.ToDouble(temperatureLabel.Text[0..^2]);
                        double fahrenheit = (temp * 9 / 5) + 32;
                        fahrenheit = Math.Round(fahrenheit, 1);
                        temperatureLabel.Text = $"{fahrenheit}°F";
                    }

                    //Make the first character of the description uppercase, and new words start with uppercase after a space
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

        // Create a method that gets the user's geolocation and returns the city name
        private static async Task<string> GetUserLocation()
        {
            string city = "";
            using HttpClient client = new();
            UriBuilder builder = new("https://ipinfo.io/json");
            HttpResponseMessage response = await client.GetAsync(builder.ToString());
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    string json = await response.Content.ReadAsStringAsync();
                    LocationInfo? info = JsonConvert.DeserializeObject<LocationInfo>(json);
                    // Search through the json file to find the city
                    JObject jsonQuery = JObject.Parse(json);
                    city = jsonQuery["city"].ToString();
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
            return city;
        }

        private void Search(object sender, EventArgs e)
        {
            string city = cityTextBox.Text;

            if (!string.IsNullOrEmpty(city))
            {
                FindWeatherDetails(city);
            }
        }

        private async void SearchByLocation()
        {
            string city = await GetUserLocation();

            if (string.IsNullOrEmpty(city))
            {
                MessageBox.Show("Please enter a city.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            FindWeatherDetails(city);
        }
    }
}