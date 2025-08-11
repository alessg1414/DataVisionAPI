using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DataVisionAPI.Models.DTOs;
using DataVisionAPI.Services;

namespace DataVisionAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogService _logService;

        public UsersController(IUserService userService, ILogService logService)
        {
            _userService = userService;
            _logService = logService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

                // Log de la consulta
                await _logService.LogConsultaAsync(userId, $"POST /api/users");

                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Datos de entrada inválidos",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });
                }

                // Validar rol
                if (!string.IsNullOrEmpty(createUserDto.Rol) &&
                    createUserDto.Rol != "User" && createUserDto.Rol != "Admin")
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "El rol debe ser 'User' o 'Admin'"
                    });
                }

                var newUser = await _userService.CreateUserAsync(createUserDto);

                if (newUser == null)
                {
                    return Conflict(new
                    {
                        Success = false,
                        Message = "El usuario ya existe"
                    });
                }

                return CreatedAtAction(nameof(GetUserInfo), new { username = newUser.Usuario_ }, new
                {
                    Success = true,
                    Message = "Usuario creado exitosamente",
                    User = new
                    {
                        Id = newUser.Id,
                        Usuario = newUser.Usuario_,
                        Rol = newUser.Rol
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Error creando usuario: {ex.Message}"
                });
            }
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyInfo()
        {
            try
            {
                var username = User.Identity?.Name;
                var rol = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
                var userId = User.FindFirst("UserId")?.Value;

                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized(new
                    {
                        Success = false,
                        Message = "Usuario no autenticado"
                    });
                }

                return Ok(new
                {
                    Success = true,
                    User = new
                    {
                        Id = userId,
                        Usuario = username,
                        Rol = rol
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Error obteniendo información del usuario: {ex.Message}"
                });
            }
        }

        [HttpGet("{username}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserInfo(string username)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

                // Log de la consulta
                await _logService.LogConsultaAsync(userId, $"GET /api/users/{username}");

                var user = await _userService.GetUserByUsernameAsync(username);

                if (user == null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = "Usuario no encontrado"
                    });
                }

                return Ok(new
                {
                    Success = true,
                    User = new
                    {
                        Id = user.Id,
                        Usuario = user.Usuario_,
                        Rol = user.Rol
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Error obteniendo información del usuario: {ex.Message}"
                });
            }
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                var username = User.Identity?.Name;

                // Log de la consulta
                await _logService.LogConsultaAsync(userId, "PUT /api/users/change-password");

                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Datos de entrada inválidos",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });
                }

                // Obtener usuario actual
                var user = await _userService.GetUserByUsernameAsync(username ?? "");
                if (user == null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = "Usuario no encontrado"
                    });
                }

                // Validar contraseña actual
                var isCurrentPasswordValid = await _userService.ValidatePasswordAsync(changePasswordDto.CurrentPassword, user.Password);
                if (!isCurrentPasswordValid)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "La contraseña actual es incorrecta"
                    });
                }

                // Actualizar contraseña
                var success = await _userService.UpdatePasswordAsync(userId, changePasswordDto.NewPassword);

                if (success)
                {
                    return Ok(new
                    {
                        Success = true,
                        Message = "Contraseña actualizada exitosamente"
                    });
                }
                else
                {
                    return StatusCode(500, new
                    {
                        Success = false,
                        Message = "Error actualizando la contraseña"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Error cambiando contraseña: {ex.Message}"
                });
            }
        }
    }
}