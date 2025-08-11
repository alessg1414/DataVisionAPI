using DataVisionAPI.Models;

namespace DataVisionAPI.Services
{
    public interface ILogService
    {
        Task LogConsultaAsync(int usuarioId, string endpoint);
        Task<List<Log>> GetLogsAsync();
        Task<List<Log>> GetLogsByUserAsync(int usuarioId);
    }
}