using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DBDHub.Models
{
    public class Killer
    {
        [BindNever]
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

        public ICollection<Perk> Perks { get; set; } = new List<Perk>();
        public ICollection<Addon> Addons { get; set; } = new List<Addon>();
        public ICollection<MapKiller> MapKillers { get; set; } = new List<MapKiller>();
    }
}