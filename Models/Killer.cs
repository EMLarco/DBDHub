using System.ComponentModel.DataAnnotations;

namespace DBDHub.Models
{
    public class Killer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Poder { get; set; }

        [Range(0, 20)]
        public decimal Velocidad { get; set; }

        [StringLength(50)]
        public string? Dificultad { get; set; }

        public string? Historia { get; set; }

        [StringLength(500)]
        public string? ImagenUrl { get; set; }   // <-- NUEVO

        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public ICollection<Perk>? Perks { get; set; }
        public ICollection<Addon>? Addons { get; set; }
        public ICollection<MapKiller>? MapKillers { get; set; }
    }
}