using System.ComponentModel.DataAnnotations;

namespace DataVisionAPI.Models
{
    public class Log
    {
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public DateTime FechaConsulta { get; set; } = DateTime.Now;

        [Required]
        [StringLength(255)]
        public string EndpointConsultado { get; set; } = string.Empty;

        // Navegación
        public Usuario Usuario { get; set; } = null!;
    }
}