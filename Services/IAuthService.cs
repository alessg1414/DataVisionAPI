using DataVisionAPI.Models.DTOs;

namespace DataVisionAPI.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        string GenerateJwtToken(string usuario, string rol, int userId);
    }
}