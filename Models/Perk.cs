using System.ComponentModel.DataAnnotations;

namespace DBDHub.Models
{
    public class Perk
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la perk es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        [Range(1, 3)]
        public int Nivel { get; set; }

        [Required(ErrorMessage = "El tipo de personaje es obligatorio")]
        [StringLength(20)]
        public string TipoPersonaje { get; set; } = string.Empty; // "Killer" o "Survivor"

        public int? KillerId { get; set; }
        public Killer? Killer { get; set; }

        public int? SurvivorId { get; set; }
        public Survivor? Survivor { get; set; }

        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        [StringLength(500)]
        public string? ImagenUrl { get; set; }
    }
}