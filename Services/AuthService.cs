
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DataVisionAPI.Data;
using DataVisionAPI.Models.DTOs;

namespace DataVisionAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                // Buscar usuario en la base de datos
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Usuario_ == loginDto.Usuario);

                if (usuario == null)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Usuario no encontrado"
                    };
                }

                // Verificar contraseña
                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, usuario.Password))
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Contraseña incorrecta"
                    };
                }

                // Generar token JWT
                var token = GenerateJwtToken(usuario.Usuario_, usuario.Rol, usuario.Id);

                return new AuthResponseDto
                {
                    Success = true,
                    Message = "Login exitoso",
                    Token = token,
                    Usuario = usuario.Usuario_,
                    Rol = usuario.Rol
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = $"Error en el login: {ex.Message}"
                };
            }
        }

        public string GenerateJwtToken(string usuario, string rol, int userId)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"] ?? "your-256-bit-secret-key-here-must-be-at-least-32-characters-long");

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario),
                new Claim(ClaimTypes.Role, rol),
                new Claim("UserId", userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, usuario),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,
                    new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(double.Parse(jwtSettings["ExpiryInHours"] ?? "24")),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}