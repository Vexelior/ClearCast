using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using static WeatherAPI.API;

namespace WeatherAPI
{
    public partial class Form1 : Form
    {
        // API Info. \\
        private const string OpenWeatherMapApiKey = "49c8545070487501c919486dbf8afdaf";
        private const string OpenWeatherMapApiUrl = "https://api.openweathermap.org/data/2.5/weather";

        public Form1()
        {
            InitializeComponent();
            SearchByLocation();

            // Set the position of the form to the center of the screen. \\
            StartPosition = FormStartPosition.CenterScreen;

            // If the cityListBox is not visible, allow the enter key to search. \\
            cityTextBox.KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.Enter && cityListBox.Visible == false)
                {
                    Search(sender, e);
                }
            };
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
                    if (locationDetails != null)
                    {
                        if (country == "United States" || country == "US")
                        {
                            cityLabel.Text = $"{locationDetails[0]}, {locationDetails[1]}";
                        }
                        else if (country != "United States" || country != "US")
                        {
                            cityLabel.Text = $"{locationDetails[0]}, {locationDetails[2]}";
                        }
                    }
                    else
                    {
                        cityLabel.Text = $"{locationDetails[0]}";
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
                        string? descriptionIcon = GetWeatherIconCode(description);
                        string imageUrl = string.Format("https://openweathermap.org/img/w/{0}.png", descriptionIcon);
                        if (description == "light rain")
                        {
                            imageUrl = string.Format("https://openweathermap.org/img/wn/{0}.png", descriptionIcon);
                        }
                        description = description.Substring(0, 1).ToUpper() + description[1..];
                        description = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(description);
                        descriptionLabel.Text = description;
                        weatherPictureBox.ImageLocation = imageUrl;

                        // If the image is not the same size as the picture box, resize it. \\
                        if (weatherPictureBox.Image != null)
                        {
                            if (weatherPictureBox.Image.Size != weatherPictureBox.Size)
                            {
                                weatherPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                            }
                        }
                    }

                    // If there is an instance of PleaseWaitForm, close it. \\
                    if (Application.OpenForms.OfType<PleaseWaitForm>().Any())
                    {
                        Application.OpenForms.OfType<PleaseWaitForm>().First().Close();
                    }
                    if (searchButton.Enabled == false && searchButton.Cursor == Cursors.No)
                    {
                        searchButton.Enabled = true;
                        searchButton.Cursor = Cursors.Default;
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage($"Error finding weather details!\n\n{ex.Message}");
                }
            }
            else
            {
                ErrorMessage("Error retrieving weather information.");
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
                    ErrorMessage($"Error finding location details!\n\n{ex.Message}");
                }
            }
            else
            {
                ErrorMessage("Error retrieving location information.");
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
        }


        private async void Search(object? sender, EventArgs e)
        {
            string city = "";
            string region = "";
            string country = "";

            // Disable the search button. \\
            searchButton.Enabled = false;
            searchButton.Cursor = Cursors.No;

            // If there is a comma in the city name, split the string into city and region. \\
            if (cityTextBox.Text.Contains(","))
            {
                string[] cityInfo = cityTextBox.Text.Split(",");
                city = cityInfo[0];
                region = cityInfo[1];
                country = cityInfo[2];

                // If there is a space after the comma, remove it. \\
                if (country.StartsWith(" "))
                {
                    country = country[1..];
                }

                if (region.StartsWith(" "))
                {
                    region = region[1..];
                }

                if (city.StartsWith(" "))
                {
                    city = city[1..];
                }
            }
            else
            {
                city = cityTextBox.Text;
            }

            if (string.IsNullOrEmpty(city))
            {
                ErrorMessage("Please enter a city.");
                searchButton.Enabled = true;
                searchButton.Cursor = Cursors.Default;
            }
            else
            {
                try
                {
                    ShowLoadingMessage();
                    List<string> locationDetails = await LocationDetails(city, region, country);
                    FindWeatherDetails(locationDetails[0], locationDetails[1], locationDetails[2]);
                }
                catch (Exception ex)
                {
                    ErrorMessage($"Error finding weather details!\n\n{ex.Message}");
                }
            }

            cityTextBox.Clear();
        }


        private static async Task<List<string>> LocationDetails(string city, string region, string country)
        {
            List<string> locationDetails = new();

            string zipCode = await GetZipCode(city, region, country);

            using HttpClient client = new();
            UriBuilder builder = new(OpenWeatherMapApiUrl);
            builder.Query = $"zip={zipCode}&appid={OpenWeatherMapApiKey}&units=metric";
            HttpResponseMessage response = client.GetAsync(builder.ToString()).Result;

            if (response != null)
            {
                locationDetails.Add(city);
                locationDetails.Add(region);
                locationDetails.Add(country);
            }

            return locationDetails;
        }


        // Create a method that gets the zip code for the city entered. \\
        private static async Task<string> GetZipCode(string city, string region, string country)
        {
            country = GetCountryCode(country).ToLower();
            if (country == "us")
            {
                region = GetStateCode(region).ToLower();
            }
            else
            {
                region = region.ToLower();
            }
            city = city.ToLower();

            // If the country is not US, use the GeoNames API to find the zip code. \\
            if (country != "us")
            {
                return GetGeoNamesZipCode(city, country);
            }


            string url = $"https://api.zippopotam.us/{country}/{region}/{city}";
            string? zipCode = "";
            string result = "";

            try
            {
                using HttpClient client = new();
                UriBuilder builder = new(url);
                HttpResponseMessage response = await client.GetAsync(builder.ToString());
                result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    ErrorMessage("Error retrieving zip code.");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage($"Error retrieving zip code!\n\n{ex.Message}");
            }

            dynamic? json = JsonConvert.DeserializeObject(result);
            zipCode = json.places[0]["post code"];

            return zipCode;
        }


        private static string GetGeoNamesZipCode(string city, string country)
        {
            const string API_KEY = "0f84326655f746d78c9c5c9552756925";
            string countryName = GetCountryCode(country.ToUpper());

            // Capitalize the first letter of the city. \\
            city = city[0].ToString().ToUpper() + city[1..];

            string url = $"https://api.opencagedata.com/geocode/v1/json?q={city},{countryName}&key={API_KEY}&language=en&pretty=1";
            string? zipCode = "";

            try
            {
                using HttpClient client = new();
                UriBuilder builder = new(url);
                HttpResponseMessage response = client.GetAsync(builder.ToString()).Result;
                string result = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                {
                    ErrorMessage("Error finding zipcode.");
                }

                // Loop through the results and find the zip code. \\
                foreach (var item in JObject.Parse(result)["results"])
                {
                    // If the first item in the JSON array under "components" matches both the city and country, return the zip code. \\
                    if (item["components"]["city"]?.ToString() == city && item["components"]["country"]?.ToString() == countryName)
                    {
                        // Get the longitude and latitude of the city and use the OpenWeatherMap API to find the zip code. \\
                        string lat = item["geometry"]["lat"].ToString();
                        string lng = item["geometry"]["lng"].ToString();
                        zipCode = GetCountryZipCode(lat, lng);
                        break;
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorMessage($"Error finding zipcode!\n\n{ex.Message}");
            }

            return zipCode;
        }


        private static string GetCountryZipCode(string lat, string lng)
        {
            string url = $"https://api.opencagedata.com/geocode/v1/json?q={lat}+{lng}&key=0f84326655f746d78c9c5c9552756925&language=en&pretty=1";
            string? zipCode = "";

            try
            {
                using HttpClient client = new();
                UriBuilder builder = new(url);
                HttpResponseMessage response = client.GetAsync(builder.ToString()).Result;
                string result = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                {
                    ErrorMessage("Error finding zipcode.");
                }

                // Loop through the results and find the zip code. \\
                foreach (var item in JObject.Parse(result)["results"])
                {
                    // If the first item in the JSON array under "components" matches both the city and country, return the zip code. \\
                    if (item["components"]["postcode"] != null)
                    {
                        zipCode = item["components"]["postcode"].ToString();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage($"Error finding zipcode!\n\n{ex.Message}");
            }

            return zipCode;
        }


        private static string GetCountryCode(string country)
        {
            string code = "";
            string url = $"https://restcountries.com/v3.1/name/{country}";

            using HttpClient client = new();
            using HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                JArray countryData = JArray.Parse(content);

                if (countryData.Count > 0)
                {
                    code = countryData[0]["cca2"].ToString();
                }
            }

            return code;
        }


        private static string GetStateCode(string state)
        {
            string code = string.Empty;

            // Create a dictionary of states and their codes. \\
            Dictionary<string, string> states = new()
            {
                { "Alabama", "AL" },
                { "Alaska", "AK" },
                { "Arizona", "AZ" },
                { "Arkansas", "AR" },
                { "California", "CA" },
                { "Colorado", "CO" },
                { "Connecticut", "CT" },
                { "Delaware", "DE" },
                { "District of Columbia", "DC" },
                { "Florida", "FL" },
                { "Georgia", "GA" },
                { "Hawaii", "HI" },
                { "Idaho", "ID" },
                { "Illinois", "IL" },
                { "Indiana", "IN" },
                { "Iowa", "IA" },
                { "Kansas", "KS" },
                { "Kentucky", "KY" },
                { "Louisiana", "LA" },
                { "Maine", "ME" },
                { "Maryland", "MD" },
                { "Massachusetts", "MA" },
                { "Michigan", "MI" },
                { "Minnesota", "MN" },
                { "Mississippi", "MS" },
                { "Missouri", "MO" },
                { "Montana", "MT" },
                { "Nebraska", "NE" },
                { "Nevada", "NV" },
                { "New Hampshire", "NH" },
                { "New Jersey", "NJ" },
                { "New Mexico", "NM" },
                { "New York", "NY" },
                { "North Carolina", "NC" },
                { "North Dakota", "ND" },
                { "Ohio", "OH" },
                { "Oklahoma", "OK" },
                { "Oregon", "OR" },
                { "Pennsylvania", "PA" },
                { "Rhode Island", "RI" },
                { "South Carolina", "SC" },
                { "South Dakota", "SD" },
                { "Tennessee", "TN" },
                { "Texas", "TX" },
                { "Utah", "UT" },
                { "Vermont", "VT" },
                { "Virginia", "VA" },
                { "Washington", "WA" },
                { "West Virginia", "WV" },
                { "Wisconsin", "WI" },
                { "Wyoming", "WY" }
            };

            // Loop through the dictionary and find the state code for the passed in state. \\
            foreach (KeyValuePair<string, string> value in states)
            {
                if (value.Key == state)
                {
                    code = value.Value;
                }
            }

            return code;
        }


        private void CityTextBox_TextChanged(object? sender, EventArgs e)
        {
            string city = cityTextBox.Text;

            // Call an API to retrieve a list of cities that match the user's input. \\
            if (!string.IsNullOrEmpty(city))
            {
                using WebClient client = new();
                List<string> cityList = new();

                try
                {
                    string json = client.DownloadString($"https://api.teleport.org/api/cities/?search={city}");
                    JObject jsonQuery = JObject.Parse(json);
                    JArray? cities = jsonQuery["_embedded"]["city:search-results"] as JArray;

                    // Display the list of cities in a listbox. \\
                    cityListBox.Items.Clear();
                    foreach (JObject cityObject in cities)
                    {
                        string cityName = cityObject["matching_full_name"].ToString();
                        cityListBox.Items.Add(cityName);

                        foreach (string item in cityListBox.Items)
                        {
                            cityList.Add(item);
                        }
                    }

                    // Write the indices of the list of cities to the console. \\


                    cityListBox.Visible = true;

                    if (cityListBox.Visible == true)
                    {
                        // While the user is typing that is not the up and down arrow keys, focus the textbox, otherwise focus the listbox. \\
                        cityTextBox.KeyDown += (s, ev) =>
                        {
                            if (ev.KeyCode != Keys.Up && ev.KeyCode != Keys.Down)
                            {
                                cityTextBox.Focus();
                            }

                            if (ev.KeyCode == Keys.Up || ev.KeyCode == Keys.Down)
                            {
                                cityListBox.Focus();
                            }
                        };

                        // Disable the up and down arrow keys from moving the cursor in the city textbox. \\
                        cityTextBox.KeyDown += (s, ev) =>
                        {
                            if (ev.KeyCode == Keys.Up || ev.KeyCode == Keys.Down)
                            {
                                ev.SuppressKeyPress = true;
                            }

                            if (ev.KeyCode == Keys.Down && cityListBox.SelectedIndex == -1)
                            {
                                cityListBox.SelectedIndex = 0;
                            }
                        };


                        // Create an event handler for selecting an item in the list with the arrow keys. \\
                        cityListBox.KeyDown += (s, ev) =>
                        {
                            int index = cityListBox.SelectedIndex;

                            if (index == -1)
                            {
                                index = 0;
                            }

                            if (ev.KeyCode == Keys.Enter)
                            {
                                // Clear the city textbox. \\
                                cityTextBox.Clear();

                                // Get the text from the index of the selected item in the listbox. \\
                                string? cityText = cityListBox.Items[index].ToString();

                                // Find the matching city in the list of cities. \\
                                foreach (string item in cityList)
                                {
                                    if (item == cityText)
                                    {
                                        // Set the city textbox to the selected city. \\
                                        cityTextBox.Text = item;
                                    }
                                }

                                // Hide the listbox. \\
                                cityListBox.Visible = false;
                            }

                            if (ev.KeyCode == Keys.Down)
                            {
                                index++;
                            }

                            if (ev.KeyCode == Keys.Up)
                            {
                                index--;
                            }
                        };

                        // Mouseleave event to hide the listbox when the user clicks outside of it. \\
                        cityListBox.MouseLeave += (s, ev) =>
                        {
                            cityListBox.Visible = false;
                        };

                        // Allow the user to click on an item in the listbox to select it. \\
                        cityListBox.MouseClick += (s, ev) =>
                        {
                            int index = cityListBox.SelectedIndex;

                            if (index == -1)
                            {
                                index = 0;
                                // Set the selected index to the first item in the listbox. \\
                                cityListBox.SelectedIndex = index;
                            }

                            // Clear the city textbox. \\
                            cityTextBox.Clear();

                            // Get the text from the index of the selected item in the listbox. \\
                            string? cityText = cityListBox.Items[index].ToString();

                            // Find the matching city in the list of cities. \\
                            foreach (string item in cityList)
                            {
                                if (item == cityText)
                                {
                                    // Set the city textbox to the selected city. \\
                                    cityTextBox.Text = item;
                                }
                            }

                            // Hide the listbox. \\
                            cityListBox.Visible = false;
                        };
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage($"Error retrieving cities!\n\n{ex.Message}");
                }
            }
            else
            {
                cityListBox.Visible = false;
            }
        }


        private static void ShowLoadingMessage()
        {
            PleaseWaitForm loadingForm = new();

            // If there is more than one instance of PleaseWaitForm, close it. \\
            if (Application.OpenForms.OfType<PleaseWaitForm>().Any())
            {
                Application.OpenForms.OfType<PleaseWaitForm>().First().Close();
            }

            // Place the PleaseWaitForm in the center of the the application. \\
            loadingForm.StartPosition = FormStartPosition.CenterScreen;

            // Show the PleaseWaitForm. \\
            loadingForm.Show();
        }


        private static void ErrorMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                MessageBox.Show(message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Error creating error message!", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Write a method to get the weather icon from the API using the description. \\
        private static string GetWeatherIconCode(string description)
        {
            // Make the first letter of each word in the description lowercase. \\
            description = description.ToLower();

            string icon = description switch
            {
                "clear sky" => "01d",
                "light rain" => "10d@2x",
                "few clouds" => "02d",
                "scattered clouds" => "03d",
                "broken clouds" => "04d",
                "shower rain" => "09d",
                "rain" => "10d",
                "thunderstorm" => "11d",
                "snow" => "13d",
                "mist" => "50d",
                _ => "01d",
            };
            return icon;
        }
    }
}