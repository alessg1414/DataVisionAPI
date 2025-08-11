using DataVisionAPI.Data;
using DataVisionAPI.Models;
using DataVisionAPI.Models.DTOs;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;

namespace DataVisionAPI.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetUserByUsernameAsync(string username)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Usuario_ == username);
        }

        public async Task<Usuario?> CreateUserAsync(CreateUserDto createUserDto)
        {
            try
            {
                // Verificar si el usuario ya existe
                var existingUser = await GetUserByUsernameAsync(createUserDto.Usuario);
                if (existingUser != null)
                {
                    return null; // Usuario ya existe
                }

                // Crear nuevo usuario con contraseña hasheada
                var newUser = new Usuario
                {
                    Usuario_ = createUserDto.Usuario,
                    Password = HashPassword(createUserDto.Password),
                    Rol = createUserDto.Rol ?? "User" // Rol por defecto
                };

                _context.Usuarios.Add(newUser);
                await _context.SaveChangesAsync();

                return newUser;
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Error creating user: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> ValidatePasswordAsync(string password, string hashedPassword)
        {
            try
            {
                return await Task.Run(() => BCrypt.Net.BCrypt.Verify(password, hashedPassword));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error validating password: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdatePasswordAsync(int userId, string newPassword)
        {
            try
            {
                var user = await _context.Usuarios.FindAsync(userId);
                if (user == null) return false;

                user.Password = HashPassword(newPassword);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating password: {ex.Message}");
                return false;
            }
        }

        public string HashPassword(string password)
        {
            // BCrypt con salt automático y work factor de 12 para mayor seguridad
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
        }
    }
}