using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DataVisionAPI.Services;

namespace DataVisionAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly ILogService _logService;

        public ReportsController(IReportService reportService, ILogService logService)
        {
            _reportService = reportService;
            _logService = logService;
        }

        [HttpGet("temperature")]
        public async Task<IActionResult> GetTemperatureReport([FromQuery] string cities = "")
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

                await _logService.LogConsultaAsync(userId, $"GET /api/reports/temperature?cities={cities}");

                List<string> cityList;
                if (string.IsNullOrEmpty(cities))
                {
                    cityList = new List<string> { "San José,CR", "Cartago,CR", "Alajuela,CR", "Puntarenas,CR" };
                }
                else
                {
                    cityList = cities.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                   .Select(c => c.Trim())
                                   .ToList();
                }

                var report = await _reportService.GenerateTemperatureReportAsync(cityList);

                return Ok(new
                {
                    Success = true,
                    Report = report,
                    Message = "Reporte de temperatura generado exitosamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Error generando reporte de temperatura: {ex.Message}"
                });
            }
        }

        [HttpGet("humidity")]
        public async Task<IActionResult> GetHumidityReport([FromQuery] string cities = "")
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

                await _logService.LogConsultaAsync(userId, $"GET /api/reports/humidity?cities={cities}");

                List<string> cityList;
                if (string.IsNullOrEmpty(cities))
                {
                    cityList = new List<string> { "San José,CR", "Cartago,CR", "Alajuela,CR", "Puntarenas,CR" };
                }
                else
                {
                    cityList = cities.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                   .Select(c => c.Trim())
                                   .ToList();
                }

                var report = await _reportService.GenerateHumidityReportAsync(cityList);

                return Ok(new
                {
                    Success = true,
                    Report = report,
                    Message = "Reporte de humedad generado exitosamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Error generando reporte de humedad: {ex.Message}"
                });
            }
        }

        [HttpGet("pressure")]
        public async Task<IActionResult> GetPressureReport([FromQuery] string cities = "")
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

                await _logService.LogConsultaAsync(userId, $"GET /api/reports/pressure?cities={cities}");

                List<string> cityList;
                if (string.IsNullOrEmpty(cities))
                {
                    cityList = new List<string> { "San José,CR", "Cartago,CR", "Alajuela,CR", "Puntarenas,CR" };
                }
                else
                {
                    cityList = cities.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                   .Select(c => c.Trim())
                                   .ToList();
                }

                var report = await _reportService.GeneratePressureReportAsync(cityList);

                return Ok(new
                {
                    Success = true,
                    Report = report,
                    Message = "Reporte de presión atmosférica generado exitosamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Error generando reporte de presión: {ex.Message}"
                });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllReports([FromQuery] string cities = "")
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

                await _logService.LogConsultaAsync(userId, $"GET /api/reports/all?cities={cities}");

                List<string> cityList;
                if (string.IsNullOrEmpty(cities))
                {
                    cityList = new List<string> { "San José,CR", "Cartago,CR", "Alajuela,CR", "Puntarenas,CR" };
                }
                else
                {
                    cityList = cities.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                   .Select(c => c.Trim())
                                   .ToList();
                }

                var temperatureReport = await _reportService.GenerateTemperatureReportAsync(cityList);
                var humidityReport = await _reportService.GenerateHumidityReportAsync(cityList);
                var pressureReport = await _reportService.GeneratePressureReportAsync(cityList);

                return Ok(new
                {
                    Success = true,
                    Reports = new
                    {
                        Temperature = temperatureReport,
                        Humidity = humidityReport,
                        Pressure = pressureReport
                    },
                    Message = "Todos los reportes generados exitosamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Error generando reportes: {ex.Message}"
                });
            }
        }
    }
}