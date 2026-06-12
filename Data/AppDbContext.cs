using Microsoft.AspNetCore.Identity.EntityFrameworkCore;   // necesario
using Microsoft.EntityFrameworkCore;
using DBDHub.Models;

namespace DBDHub.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>   // cambio importante
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Tablas
        public DbSet<Killer> Killers { get; set; }
        public DbSet<Survivor> Survivors { get; set; }
        public DbSet<Perk> Perks { get; set; }
        public DbSet<Addon> Addons { get; set; }
        public DbSet<Map> Maps { get; set; }
        public DbSet<MapKiller> MapKillers { get; set; }
        public DbSet<Match> Matches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);   // ¡importante para Identity!

            // Relación Perk → Killer (1:N)
            modelBuilder.Entity<Perk>()
                .HasOne(p => p.Killer)
                .WithMany(k => k.Perks)
                .HasForeignKey(p => p.KillerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Perk → Survivor (1:N)
            modelBuilder.Entity<Perk>()
                .HasOne(p => p.Survivor)
                .WithMany(s => s.Perks)
                .HasForeignKey(p => p.SurvivorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Addon → Killer (1:N)
            modelBuilder.Entity<Addon>()
                .HasOne(a => a.Killer)
                .WithMany(k => k.Addons)
                .HasForeignKey(a => a.KillerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Auto-referencia Map → Map (variante / original)
            modelBuilder.Entity<Map>()
                .HasOne(m => m.OriginalMap)
                .WithMany(m => m.Variantes)
                .HasForeignKey(m => m.OriginalMapId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación muchos a muchos: MapKiller → Map y Killer
            modelBuilder.Entity<MapKiller>()
                .HasOne(mk => mk.Map)
                .WithMany(m => m.MapKillers)
                .HasForeignKey(mk => mk.MapId);

            modelBuilder.Entity<MapKiller>()
                .HasOne(mk => mk.Killer)
                .WithMany(k => k.MapKillers)
                .HasForeignKey(mk => mk.KillerId);

            // Relaciones de Match
            modelBuilder.Entity<Match>()
                .HasOne(m => m.Killer)
                .WithMany()
                .HasForeignKey(m => m.KillerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Survivor1)
                .WithMany()
                .HasForeignKey(m => m.Survivor1Id)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Survivor2)
                .WithMany()
                .HasForeignKey(m => m.Survivor2Id)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Survivor3)
                .WithMany()
                .HasForeignKey(m => m.Survivor3Id)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Survivor4)
                .WithMany()
                .HasForeignKey(m => m.Survivor4Id)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Map)
                .WithMany()
                .HasForeignKey(m => m.MapId)
                .OnDelete(DeleteBehavior.SetNull);

            // Índices para mejorar el rendimiento en consultas masivas
            modelBuilder.Entity<Killer>().HasIndex(k => k.Nombre);
            modelBuilder.Entity<Survivor>().HasIndex(s => s.Nombre);
            modelBuilder.Entity<Map>().HasIndex(m => m.Nombre);
            modelBuilder.Entity<Perk>().HasIndex(p => p.Nombre);
        }
    }
}