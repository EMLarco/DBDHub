using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBDHub.Models
{
    public class Match
    {
        public int Id { get; set; }

        [Required]
        public DateTime FechaPartida { get; set; }

        // Killer
        public int? KillerId { get; set; }
        public Killer? Killer { get; set; }

        // Mapa
        public int? MapId { get; set; }
        public Map? Map { get; set; }

        // 4 survivors
        public int? Survivor1Id { get; set; }
        public Survivor? Survivor1 { get; set; }

        public int? Survivor2Id { get; set; }
        public Survivor? Survivor2 { get; set; }

        public int? Survivor3Id { get; set; }
        public Survivor? Survivor3 { get; set; }

        public int? Survivor4Id { get; set; }
        public Survivor? Survivor4 { get; set; }

        // Puntuación y detalles
        public int TotalScore { get; set; }

        [StringLength(200)]
        public string MapName { get; set; } = string.Empty;

        [StringLength(200)]
        public string Location { get; set; } = string.Empty;

        public int Detail1 { get; set; }
        public int Detail2 { get; set; }
        public int Detail3 { get; set; }
        public int Detail4 { get; set; }

        [StringLength(50)]
        public string Result { get; set; } = string.Empty;

        public int DuracionSegundos { get; set; }

        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }
}