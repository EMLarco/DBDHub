using System.ComponentModel.DataAnnotations;

namespace DBDHub.Models
{
    public class Survivor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        public string? Historia { get; set; }

        [StringLength(50)]
        public string? Dificultad { get; set; }

        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public ICollection<Perk>? Perks { get; set; }

        [StringLength(500)]
        public string? ImagenUrl { get; set; }
    }
}