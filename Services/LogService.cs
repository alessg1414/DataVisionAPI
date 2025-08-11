using Microsoft.EntityFrameworkCore;
using DataVisionAPI.Data;
using DataVisionAPI.Models;

namespace DataVisionAPI.Services
{
    public class LogService : ILogService
    {
        private readonly ApplicationDbContext _context;

        public LogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogConsultaAsync(int usuarioId, string endpoint)
        {
            try
            {
                var log = new Log
                {
                    UsuarioId = usuarioId,
                    EndpointConsultado = endpoint,
                    FechaConsulta = DateTime.Now
                };

                _context.Logs.Add(log);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Error logging consultation: {ex.Message}");
            }
        }

        public async Task<List<Log>> GetLogsAsync()
        {
            return await _context.Logs
                .Include(l => l.Usuario)
                .OrderByDescending(l => l.FechaConsulta)
                .ToListAsync();
        }

        public async Task<List<Log>> GetLogsByUserAsync(int usuarioId)
        {
            return await _context.Logs
                .Include(l => l.Usuario)
                .Where(l => l.UsuarioId == usuarioId)
                .OrderByDescending(l => l.FechaConsulta)
                .ToListAsync();
        }
    }
}