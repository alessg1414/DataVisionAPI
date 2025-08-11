using Microsoft.AspNetCore.Mvc;
using DataVisionAPI.Models.DTOs;
using DataVisionAPI.Services;

namespace DataVisionAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new AuthResponseDto
                    {
                        Success = false,
                        Message = "Datos de entrada inválidos"
                    });
                }

                var result = await _authService.LoginAsync(loginDto);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return Unauthorized(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AuthResponseDto
                {
                    Success = false,
                    Message = $"Error interno del servidor: {ex.Message}"
                });
            }
        }

        [HttpGet("validate")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public IActionResult ValidateToken()
        {
            try
            {
                var usuario = User.Identity?.Name;
                var rol = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
                var userId = User.FindFirst("UserId")?.Value;

                return Ok(new
                {
                    Success = true,
                    Usuario = usuario,
                    Rol = rol,
                    UserId = userId,
                    Message = "Token válido"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Error validando token: {ex.Message}"
                });
            }
        }
    }
}