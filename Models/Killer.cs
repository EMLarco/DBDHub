using System.ComponentModel.DataAnnotations;

namespace DBDHub.Models
{
    public class Killer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Poder { get; set; }

        [Range(0, 20, ErrorMessage = "La velocidad debe estar entre 0 y 20")]
        public decimal Velocidad { get; set; }

        [StringLength(50)]
        public string? Dificultad { get; set; }

        public string? Historia { get; set; }

        // Campo de eliminación lógica
        public bool Activo { get; set; } = true;

        // Campo de auditoría
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Relaciones
        public ICollection<Perk>? Perks { get; set; }
        public ICollection<Addon>? Addons { get; set; }
        public ICollection<MapKiller>? MapKillers { get; set; }
    }
}