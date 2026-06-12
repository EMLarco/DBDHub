using System.ComponentModel.DataAnnotations;

namespace DBDHub.Models
{
    public class Map
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del mapa es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(200)]
        public string? ImagenUrl { get; set; }

        [StringLength(50)]
        public string? Tipo { get; set; } // "Original" o "Variante"

        // Auto-referencia para variantes
        public int? OriginalMapId { get; set; }
        public Map? OriginalMap { get; set; }

        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public ICollection<MapKiller>? MapKillers { get; set; }
        public ICollection<Map>? Variantes { get; set; }
    }
}