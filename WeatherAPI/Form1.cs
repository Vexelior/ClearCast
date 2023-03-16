using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;
using static WeatherAPI.API;
using System.Windows.Threading;
using System.Text.Json;
using System.IO;

namespace WeatherAPI
{
    public partial class Form1 : Form
    {
        // Timer Object. \\
        private readonly DispatcherTimer timer;

        // API Info. \\
        private const string OpenWeatherMapApiKey = "49c8545070487501c919486dbf8afdaf";
        private const string OpenWeatherMapApiUrl = "https://api.openweathermap.org/data/2.5/weather";

        public Form1()
        {
            InitializeComponent();
            SearchByLocation();

            // Timer logic. \\
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Timer_Tick;
            timer.Start();

            // Allow enter to be pressed to call the function from the text box. \\
            cityTextBox.KeyDown += SearchBox_KeyDown;
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
                    MessageBox.Show($"Error finding weather details!\n\n{ex.Message}", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show($"Error finding geolocation!\n\n{ex.Message}", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private async void Search(object? sender, EventArgs e)
        {
            string city = "";
            string region = "";
            string country = "";

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

            if (!string.IsNullOrEmpty(city))
            {
                List<string> locationDetails = await LocationDetails(city, region, country);
                FindWeatherDetails(locationDetails[0], locationDetails[1], locationDetails[2]);
            }
            else
            {
                MessageBox.Show("Please enter a city.", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    locationDetails.Add(city);
                    locationDetails.Add(region);
                    locationDetails.Add(country);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error retrieving location detials!\n\n{ex.Message}", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Error retrieving weather information.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                return await GetGeoNamesZipCode(city, region, country);
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
                    MessageBox.Show("Error retrieving weather information.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error finding zipcode!\n\n{ex.Message}", "Oops!" ,MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            dynamic? json = JsonConvert.DeserializeObject(result);
            zipCode = json.places[0]["post code"];

            if (string.IsNullOrEmpty(zipCode))
            {
                zipCode = "null";
                MessageBox.Show($"Error finding zipcode!", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return zipCode;
        }

        private static Task<string> GetGeoNamesZipCode(string city, string region, string country)
        {
            string countryName = country.ToUpper();
            countryName = GetCountryCode(countryName);
            // Get the lat and long for the city entered. \\
            string latlongUrl = $"http://api.geonames.org/searchJSON?q={city}&country={countryName}&username=vexelior";
            string? lat = "";
            string? lng = "";
            string result = "";

            try
            {
                using HttpClient client = new();
                UriBuilder builder = new(latlongUrl);
                HttpResponseMessage response = client.GetAsync(builder.ToString()).Result;
                result = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Error retrieving weather information.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error finding zipcode!\n\n{ex.Message}", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            dynamic? json = JsonConvert.DeserializeObject(result);
            lat = json.geonames[0].lat;
            lng = json.geonames[0].lng;

            // Get the zip code for the lat and long. \\
            string url = $"http://api.geonames.org/findNearbyPostalCodesJSON?lat={lat}&lng={lng}&username=vexelior";
            string? zipCode = "";
            string newResult = "";

            try
            {
                using HttpClient client = new();
                UriBuilder builder = new(url);
                HttpResponseMessage response = client.GetAsync(builder.ToString()).Result;
                newResult = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Error retrieving weather information.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error finding zipcode!\n\n{ex.Message}", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            dynamic? convJson = JsonConvert.DeserializeObject(result);
            zipCode = convJson.postalCodes[0].postalCode;

            if (string.IsNullOrEmpty(zipCode))
            {
                zipCode = "null";
                MessageBox.Show($"Error finding zipcode!", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return Task.FromResult(zipCode);
        }

        private static string GetCountryCode(string country)
        {
            string code = "";

            Dictionary<string, string> countries = new()
            {
                { "Afghanistan", "AF" },
                { "Åland Islands", "AX" },
                { "Albania", "AL" },
                { "Algeria", "DZ" },
                { "American Samoa", "AS" },
                { "Andorra", "AD" },
                { "Angola", "AO" },
                { "Anguilla", "AI" },
                { "Antarctica", "AQ" },
                { "Antigua and Barbuda", "AG" },
                { "Argentina", "AR" },
                { "Armenia", "AM" },
                { "Aruba", "AW" },
                { "Australia", "AU" },
                { "Austria", "AT" },
                { "Azerbaijan", "AZ" },
                { "Bahamas", "BS" },
                { "Bahrain", "BH" },
                { "Bangladesh", "BD" },
                { "Barbados", "BB" },
                { "Belarus", "BY" },
                { "Belgium", "BE" },
                { "Belize", "BZ" },
                { "Benin", "BJ" },
                { "Bermuda", "BM" },
                { "Bhutan", "BT" },
                { "Bolivia, Plurinational State of", "BO" },
                { "Bonaire, Sint Eustatius and Saba", "BQ" },
                { "Bosnia and Herzegovina", "BA" },
                { "Botswana", "BW" },
                { "Bouvet Island", "BV" },
                { "Brazil", "BR" },
                { "British Indian Ocean Territory", "IO" },
                { "Brunei Darussalam", "BN" },
                { "Bulgaria", "BG" },
                { "Burkina Faso", "BF" },
                { "Burundi", "BI" },
                { "Cambodia", "KH" },
                { "Cameroon", "CM" },
                { "Canada", "CA" },
                { "Cape Verde", "CV" },
                { "Cayman Islands", "KY" },
                { "Central African Republic", "CF" },
                { "Chad", "TD" },
                { "Chile", "CL" },
                { "China", "CN" },
                { "Christmas Island", "CX" },
                { "Cocos (Keeling) Islands", "CC" },
                { "Colombia", "CO" },
                { "Comoros", "KM" },
                { "Congo", "CG" },
                { "Congo, the Democratic Republic of the", "CD" },
                { "Cook Islands", "CK" },
                { "Costa Rica", "CR" },
                { "Côte d'Ivoire", "CI" },
                { "Croatia", "HR" },
                { "Cuba", "CU" },
                { "Curaçao", "CW" },
                { "Cyprus", "CY" },
                { "Czech Republic", "CZ" },
                { "Denmark", "DK" },
                { "Djibouti", "DJ" },
                { "Dominica", "DM" },
                { "Dominican Republic", "DO" },
                { "Ecuador", "EC" },
                { "Greece", "GR" },
                { "Greenland", "GL" },
                { "Grenada", "GD" },
                { "Guadeloupe", "GP" },
                { "Guam", "GU" },
                { "Guatemala", "GT" },
                { "Guernsey", "GG" },
                { "Guinea", "GN" },
                { "Guinea-Bissau", "GW" },
                { "Guyana", "GY" },
                { "Haiti", "HT" },
                { "Heard Island and McDonald Islands", "HM" },
                { "Holy See", "VA" },
                { "Honduras", "HN" },
                { "Hong Kong", "HK" },
                { "Hungary", "HU" },
                { "Iceland", "IS" },
                { "India", "IN" },
                { "Indonesia", "ID" },
                { "Iran, Islamic Republic of", "IR" },
                { "Iraq", "IQ" },
                { "Ireland", "IE" },
                { "Isle of Man", "IM" },
                { "Israel", "IL" },
                { "Italy", "IT" },
                { "Jamaica", "JM" },
                { "Japan", "JP" },
                { "Jersey", "JE" },
                { "Jordan", "JO" },
                { "Kazakhstan", "KZ" },
                { "Kenya", "KE" },
                { "Kiribati", "KI" },
                { "Korea, Democratic People's Republic of", "KP" },
                { "Korea, Republic of", "KR" },
                { "Kuwait", "KW" },
                { "Kyrgyzstan", "KG" },
                { "Lao People's Democratic Republic", "LA" },
                { "Latvia", "LV" },
                { "Lebanon", "LB" },
                { "Lesotho", "LS" },
                { "Liberia", "LR" },
                { "Libya", "LY" },
                { "Liechtenstein", "LI" },
                { "Lithuania", "LT" },
                { "Luxembourg", "LU" },
                { "Macao", "MO" },
                { "Macedonia, the former Yugoslav Republic of", "MK" },
                { "Madagascar", "MG" },
                { "Malawi", "MW" },
                { "Malaysia", "MY" },
                { "Maldives", "MV" },
                { "Mali", "ML" },
                { "Malta", "MT" },
                { "Marshall Islands", "MH" },
                { "Martinique", "MQ" },
                { "Mauritania", "MR" },
                { "Mauritius", "MU" },
                { "Mayotte", "YT" },
                { "Mexico", "MX" },
                { "Micronesia, Federated States of", "FM" },
                { "Moldova, Republic of", "MD" },
                { "Monaco", "MC" },
                { "Mongolia", "MN" },
                { "Montenegro", "ME" },
                { "Montserrat", "MS" },
                { "Morocco", "MA" },
                { "Mozambique", "MZ" },
                { "Myanmar", "MM" },
                { "Namibia", "NA" },
                { "Nauru", "NR" },
                { "Nepal", "NP" },
                { "Netherlands", "NL" },
                { "New Caledonia", "NC" },
                { "New Zealand", "NZ" },
                { "Nicaragua", "NI" },
                { "Niger", "NE" },
                { "Nigeria", "NG" },
                { "Niue", "NU" },
                { "Norfolk Island", "NF" },
                { "Northern Mariana Islands", "MP" },
                { "Norway", "NO" },
                { "Oman", "OM" },
                { "Pakistan", "PK" },
                { "Palau", "PW" },
                { "Palestine, State of", "PS" },
                { "Panama", "PA" },
                { "Papua New Guinea", "PG" },
                { "Paraguay", "PY" },
                { "Peru", "PE" },
                { "Philippines", "PH" },
                { "Pitcairn", "PN" },
                { "Poland", "PL" },
                { "Portugal", "PT" },
                { "Puerto Rico", "PR" },
                { "Qatar", "QA" },
                { "Réunion", "RE" },
                { "Romania", "RO" },
                { "Russian Federation", "RU" },
                { "Rwanda", "RW" },
                { "Saint Barthélemy", "BL" },
                { "Saint Helena, Ascension and Tristan da Cunha", "SH" },
                { "Saint Kitts and Nevis", "KN" },
                { "Saint Lucia", "LC" },
                { "Saint Martin(French part)", "MF" },
                { "Saint Pierre and Miquelon", "PM" },
                { "Saint Vincent and the Grenadines", "VC" },
                { "Samoa", "WS" },
                { "San Marino", "SM" },
                { "Sao Tome and Principe", "ST" },
                { "Saudi Arabia", "SA" },
                { "Senegal", "SN" },
                { "Serbia", "RS" },
                { "Seychelles", "SC" },
                { "Sierra Leone", "SL" },
                { "Singapore", "SG" },
                { "Sint Maarten(Dutch part)", "SX" },
                { "Slovakia", "SK" },
                { "Slovenia", "SI" },
                { "Solomon Islands", "SB" },
                { "Somalia", "SO" },
                { "South Africa", "ZA" },
                { "South Georgia and the South Sandwich Islands", "GS" },
                { "South Sudan", "SS" },
                { "Spain", "ES" },
                { "Sri Lanka", "LK" },
                { "Sudan", "SD" },
                { "Suriname", "SR" },
                { "Svalbard and Jan Mayen", "SJ" },
                { "Swaziland", "SZ" },
                { "Sweden", "SE" },
                { "Switzerland", "CH" },
                { "Syrian Arab Republic", "SY" },
                { "Taiwan, Province of China", "TW" },
                { "Tajikistan", "TJ" },
                { "Tanzania, United Republic of", "TZ" },
                { "Thailand", "TH" },
                { "Timor-Leste", "TL" },
                { "Togo", "TG" },
                { "Tokelau", "TK" },
                { "Tonga", "TO" },
                { "Trinidad and Tobago", "TT" },
                { "Tunisia", "TN" },
                { "Turkey", "TR" },
                { "Turkmenistan", "TM" },
                { "Turks and Caicos Islands", "TC" },
                { "Tuvalu", "TV" },
                { "Uganda", "UG" },
                { "Ukraine", "UA" },
                { "United Arab Emirates", "AE" },
                { "United Kingdom", "GB" },
                { "United States", "US" },
                { "United States Minor Outlying Islands", "UM" },
                { "Uruguay", "UY" },
                { "Uzbekistan", "UZ" },
                { "Vanuatu", "VU" },
                { "Venezuela, Bolivarian Republic of", "VE" },
                { "Viet Nam", "VN" },
                { "Virgin Islands, British", "VG" },
                { "Virgin Islands, U.S.", "VI" },
                { "Wallis and Futuna", "WF" },
                { "Western Sahara", "EH" },
                { "Yemen", "YE" },
                { "Zambia", "ZM" },
                { "Zimbabwe", "ZW" }
            };

            if (country.Length == 2)
            {
                foreach (KeyValuePair<string, string> value in countries)
                {
                    if (value.Value == country)
                    {
                        code = value.Key;
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<string, string> value in countries)
                {
                    if (value.Key == country)
                    {
                        code = value.Value;
                    }
                }
            }

            return code;
        }

        // Create a similar method above to get the state code, United States Only. \\
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

                        // Get the total number of cities in the list. \\
                        List<string> cityList = new();

                        foreach (string item in cityListBox.Items)
                        {
                            cityList.Add(item);
                        }
                    }

                    cityListBox.Visible = true;

                    // Mouseleave event to hide the listbox when the user clicks outside of it. \\
                    cityListBox.MouseLeave += (s, e) =>
                    {
                        cityListBox.Visible = false;
                    };
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error finding cities!\n\n{ex.Message}", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                cityListBox.Visible = false;
            }
        }

        private void CityListBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            cityTextBox.Text = cityListBox.SelectedItem.ToString();
            cityListBox.Visible = false;
        }

        private void SearchBox_KeyDown(object? sender, KeyEventArgs e)
        {
            // Check if the pressed key is the enter key. \\
            if (e.KeyCode == Keys.Enter)
            {
                // If sender is null, return. \\
                if (sender == null)
                {
                    return;
                }
                else
                {
                    // Otherwise, call the search method. \\
                    Search(sender, e);
                }
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            timeLabel.Text = DateTime.Now.ToString("h:mm:ss tt") + " " + DateTime.Now.ToString("dddd, MMMM dd, yyyy");
        }
    }
}