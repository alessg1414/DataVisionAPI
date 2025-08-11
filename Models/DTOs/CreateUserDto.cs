using System.ComponentModel.DataAnnotations;

namespace DataVisionAPI.Models.DTOs
{
    public class CreateUserDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Usuario { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Rol { get; set; } = "User";
    }
}