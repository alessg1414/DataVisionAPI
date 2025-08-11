using DataVisionAPI.Models.DTOs;
using DataVisionAPI.Models;

namespace DataVisionAPI.Services
{
    public interface IUserService
    {
        Task<Usuario?> GetUserByUsernameAsync(string username);
        Task<Usuario?> CreateUserAsync(CreateUserDto createUserDto);
        Task<bool> ValidatePasswordAsync(string password, string hashedPassword);
        Task<bool> UpdatePasswordAsync(int userId, string newPassword);
        string HashPassword(string password);
    }
}