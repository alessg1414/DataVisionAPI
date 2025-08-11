using Newtonsoft.Json;
using DataVisionAPI.Models.DTOs;

namespace DataVisionAPI.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _apiKey = _configuration["OpenWeatherMap:ApiKey"] ?? "your-api-key-here";
            _baseUrl = "https://api.openweathermap.org/data/2.5/weather";
        }

        public async Task<WeatherDataDto?> GetWeatherDataAsync(string city)
        {
            try
            {
                var url = $"{_baseUrl}?q={city}&appid={_apiKey}&units=metric";
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var weatherResponse = JsonConvert.DeserializeObject<OpenWeatherMapResponse>(content);

                    if (weatherResponse != null)
                    {
                        return new WeatherDataDto
                        {
                            City = weatherResponse.Name,
                            Temperature = weatherResponse.Main.Temp,
                            Humidity = weatherResponse.Main.Humidity,
                            Pressure = weatherResponse.Main.Pressure,
                            Description = weatherResponse.Weather.FirstOrDefault()?.Description ?? "N/A",
                            Timestamp = DateTime.Now
                        };
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                // Log error (aquí podrías usar un logger)
                Console.WriteLine($"Error getting weather data for {city}: {ex.Message}");
                return null;
            }
        }

        public async Task<List<WeatherDataDto>> GetMultipleCitiesWeatherAsync(List<string> cities)
        {
            var weatherDataList = new List<WeatherDataDto>();

            var tasks = cities.Select(GetWeatherDataAsync).ToArray();
            var results = await Task.WhenAll(tasks);

            foreach (var result in results)
            {
                if (result != null)
                {
                    weatherDataList.Add(result);
                }
            }

            return weatherDataList;
        }
    }

    // Clases para deserializar la respuesta de OpenWeatherMap
    public class OpenWeatherMapResponse
    {
        public string Name { get; set; } = string.Empty;
        public MainData Main { get; set; } = new MainData();
        public List<WeatherInfo> Weather { get; set; } = new List<WeatherInfo>();
    }

    public class MainData
    {
        public double Temp { get; set; }
        public double Humidity { get; set; }
        public double Pressure { get; set; }
    }

    public class WeatherInfo
    {
        public string Description { get; set; } = string.Empty;
    }
}