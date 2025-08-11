using DataVisionAPI.Models.DTOs;

namespace DataVisionAPI.Services
{
    public interface IReportService
    {
        Task<ReportDto> GenerateTemperatureReportAsync(List<string> cities);
        Task<ReportDto> GenerateHumidityReportAsync(List<string> cities);
        Task<ReportDto> GeneratePressureReportAsync(List<string> cities);
    }
}