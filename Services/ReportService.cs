using DataVisionAPI.Models.DTOs;

namespace DataVisionAPI.Services
{
    public class ReportService : IReportService
    {
        private readonly IWeatherService _weatherService;

        public ReportService(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        public async Task<ReportDto> GenerateTemperatureReportAsync(List<string> cities)
        {
            var weatherData = await _weatherService.GetMultipleCitiesWeatherAsync(cities);

            var chartData = weatherData.Select(w => new ChartDataPoint
            {
                Label = w.City,
                Value = w.Temperature,
                Category = "Temperature (°C)"
            }).ToList();

            return new ReportDto
            {
                Type = "temperature",
                Data = chartData,
                GeneratedAt = DateTime.Now
            };
        }

        public async Task<ReportDto> GenerateHumidityReportAsync(List<string> cities)
        {
            var weatherData = await _weatherService.GetMultipleCitiesWeatherAsync(cities);

            var chartData = weatherData.Select(w => new ChartDataPoint
            {
                Label = w.City,
                Value = w.Humidity,
                Category = "Humidity (%)"
            }).ToList();

            return new ReportDto
            {
                Type = "humidity",
                Data = chartData,
                GeneratedAt = DateTime.Now
            };
        }

        public async Task<ReportDto> GeneratePressureReportAsync(List<string> cities)
        {
            var weatherData = await _weatherService.GetMultipleCitiesWeatherAsync(cities);

            var chartData = weatherData.Select(w => new ChartDataPoint
            {
                Label = w.City,
                Value = w.Pressure,
                Category = "Pressure (hPa)"
            }).ToList();

            return new ReportDto
            {
                Type = "pressure",
                Data = chartData,
                GeneratedAt = DateTime.Now
            };
        }
    }
}