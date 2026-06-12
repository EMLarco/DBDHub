using System.ComponentModel.DataAnnotations;

namespace DBDHub.Models
{
    public class MapKiller
    {
        public int Id { get; set; }

        public int MapId { get; set; }
        public Map? Map { get; set; }

        public int KillerId { get; set; }
        public Killer? Killer { get; set; }

        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }
}