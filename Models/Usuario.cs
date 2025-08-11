using System.ComponentModel.DataAnnotations;

namespace DataVisionAPI.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Usuario_ { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Rol { get; set; } = string.Empty;

        // Navegación
        public ICollection<Log> Logs { get; set; } = new List<Log>();
    }
}