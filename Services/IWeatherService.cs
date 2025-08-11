using DataVisionAPI.Models.DTOs;

namespace DataVisionAPI.Services
{
    public interface IWeatherService
    {
        Task<WeatherDataDto?> GetWeatherDataAsync(string city);
        Task<List<WeatherDataDto>> GetMultipleCitiesWeatherAsync(List<string> cities);
    }
}