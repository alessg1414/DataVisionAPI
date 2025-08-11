using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DataVisionAPI.Services;

namespace DataVisionAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly ILogService _logService;

        public WeatherController(IWeatherService weatherService, ILogService logService)
        {
            _weatherService = weatherService;
            _logService = logService;
        }

        [HttpGet("city/{city}")]
        public async Task<IActionResult> GetWeatherByCity(string city)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

                await _logService.LogConsultaAsync(userId, $"GET /api/weather/city/{city}");

                var weatherData = await _weatherService.GetWeatherDataAsync(city);

                if (weatherData != null)
                {
                    return Ok(new
                    {
                        Success = true,
                        Data = weatherData,
                        Message = "Datos obtenidos exitosamente"
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = $"No se encontraron datos para la ciudad: {city}"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Error obteniendo datos meteorológicos: {ex.Message}"
                });
            }
        }

        [HttpGet("cities")]
        public async Task<IActionResult> GetWeatherByCities([FromQuery] string cities)
        {
            try
            {
                if (string.IsNullOrEmpty(cities))
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Debe proporcionar al menos una ciudad"
                    });
                }

                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

                await _logService.LogConsultaAsync(userId, $"GET /api/weather/cities?cities={cities}");

                var cityList = cities.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                   .Select(c => c.Trim())
                                   .ToList();

                var weatherData = await _weatherService.GetMultipleCitiesWeatherAsync(cityList);

                return Ok(new
                {
                    Success = true,
                    Data = weatherData,
                    Count = weatherData.Count,
                    Message = "Datos obtenidos exitosamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Error obteniendo datos meteorológicos: {ex.Message}"
                });
            }
        }

        [HttpGet("costa-rica")]
        public async Task<IActionResult> GetCostaRicaWeather()
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

                await _logService.LogConsultaAsync(userId, "GET /api/weather/costa-rica");

                var costaRicaCities = new List<string>
                {
                    "San José,CR",
                    "Cartago,CR",
                    "Alajuela,CR",
                    "Puntarenas,CR",
                    "Limón,CR",
                    "Heredia,CR",
                    "Guanacaste,CR"
                };

                var weatherData = await _weatherService.GetMultipleCitiesWeatherAsync(costaRicaCities);

                return Ok(new
                {
                    Success = true,
                    Data = weatherData,
                    Count = weatherData.Count,
                    Message = "Datos meteorológicos de Costa Rica obtenidos exitosamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Error obteniendo datos meteorológicos: {ex.Message}"
                });
            }
        }
    }
}