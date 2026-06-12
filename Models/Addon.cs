using System.ComponentModel.DataAnnotations;

namespace DBDHub.Models
{
    public class Addon
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del addon es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Rareza { get; set; }

        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El killer es obligatorio")]
        public int KillerId { get; set; }
        public Killer? Killer { get; set; }

        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }
}