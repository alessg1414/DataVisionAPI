using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DataVisionAPI.Services;

namespace DataVisionAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LogsController : ControllerBase
    {
        private readonly ILogService _logService;

        public LogsController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllLogs()
        {
            try
            {
                var logs = await _logService.GetLogsAsync();

                var logsData = logs.Select(l => new
                {
                    l.Id,
                    l.UsuarioId,
                    Usuario = l.Usuario.Usuario_,
                    l.FechaConsulta,
                    l.EndpointConsultado
                }).ToList();

                return Ok(new
                {
                    Success = true,
                    Data = logsData,
                    Count = logs.Count,
                    Message = "Logs obtenidos exitosamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Error obteniendo logs: {ex.Message}"
                });
            }
        }

        [HttpGet("my-logs")]
        public async Task<IActionResult> GetMyLogs()
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                var logs = await _logService.GetLogsByUserAsync(userId);

                var logsData = logs.Select(l => new
                {
                    l.Id,
                    l.FechaConsulta,
                    l.EndpointConsultado
                }).ToList();

                return Ok(new
                {
                    Success = true,
                    Data = logsData,
                    Count = logs.Count,
                    Message = "Logs del usuario obtenidos exitosamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Error obteniendo logs del usuario: {ex.Message}"
                });
            }
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserLogs(int userId)
        {
            try
            {
                var logs = await _logService.GetLogsByUserAsync(userId);

                var logsData = logs.Select(l => new
                {
                    l.Id,
                    l.UsuarioId,
                    Usuario = l.Usuario.Usuario_,
                    l.FechaConsulta,
                    l.EndpointConsultado
                }).ToList();

                return Ok(new
                {
                    Success = true,
                    Data = logsData,
                    Count = logs.Count,
                    UserId = userId,
                    Message = $"Logs del usuario {userId} obtenidos exitosamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Error obteniendo logs del usuario: {ex.Message}"
                });
            }
        }

        [HttpGet("statistics")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetLogStatistics()
        {
            try
            {
                var allLogs = await _logService.GetLogsAsync();

                var statistics = new
                {
                    TotalConsultas = allLogs.Count,
                    ConsultasHoy = allLogs.Count(l => l.FechaConsulta.Date == DateTime.Today),
                    ConsultasUltimaSemana = allLogs.Count(l => l.FechaConsulta >= DateTime.Now.AddDays(-7)),
                    ConsultasUltimoMes = allLogs.Count(l => l.FechaConsulta >= DateTime.Now.AddDays(-30)),
                    EndpointsMasConsultados = allLogs
                        .GroupBy(l => l.EndpointConsultado)
                        .OrderByDescending(g => g.Count())
                        .Take(10)
                        .Select(g => new { Endpoint = g.Key, Consultas = g.Count() })
                        .ToList(),
                    UsuariosMasActivos = allLogs
                        .GroupBy(l => new { l.UsuarioId, l.Usuario.Usuario_ })
                        .OrderByDescending(g => g.Count())
                        .Take(10)
                        .Select(g => new {
                            UsuarioId = g.Key.UsuarioId,
                            Usuario = g.Key.Usuario_,
                            Consultas = g.Count()
                        })
                        .ToList(),
                    ConsultasPorDia = allLogs
                        .Where(l => l.FechaConsulta >= DateTime.Now.AddDays(-30))
                        .GroupBy(l => l.FechaConsulta.Date)
                        .OrderBy(g => g.Key)
                        .Select(g => new { Fecha = g.Key.ToString("yyyy-MM-dd"), Consultas = g.Count() })
                        .ToList()
                };

                return Ok(new
                {
                    Success = true,
                    Data = statistics,
                    Message = "Estadísticas obtenidas exitosamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Error obteniendo estadísticas: {ex.Message}"
                });
            }
        }
    }
}