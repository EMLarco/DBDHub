
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;   // <-- añadir esto
using Microsoft.EntityFrameworkCore;
using DBDHub.Data;
using DBDHub.Models;

namespace DBDHub.Controllers
{
    public class MatchesController : Controller
    {
        private readonly AppDbContext _context;

        public MatchesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Matches (con filtros y paginación)
        public async Task<IActionResult> Index(int? killerId, int? mapId, int? survivorId, int pagina = 1, int tamano = 5)
        {
            var query = _context.Matches
                .Include(m => m.Killer)
                .Include(m => m.Map)
                .Where(m => m.Activo == true);

            // Filtro por Killer
            if (killerId.HasValue)
                query = query.Where(m => m.KillerId == killerId.Value);

            // Filtro por Mapa
            if (mapId.HasValue)
                query = query.Where(m => m.MapId == mapId.Value);

            // Filtro por Superviviente (cualquiera de los 4)
            if (survivorId.HasValue)
                query = query.Where(m =>
                    m.Survivor1Id == survivorId.Value ||
                    m.Survivor2Id == survivorId.Value ||
                    m.Survivor3Id == survivorId.Value ||
                    m.Survivor4Id == survivorId.Value);

            int total = await query.CountAsync();
            var matches = await query
                .OrderByDescending(m => m.FechaPartida)
                .Skip((pagina - 1) * tamano)
                .Take(tamano)
                .ToListAsync();

            // Cargar listas para los dropdowns
            ViewBag.KillerList = new SelectList(_context.Killers.Where(k => k.Activo).OrderBy(k => k.Nombre), "Id", "Nombre", killerId);
            ViewBag.MapList = new SelectList(_context.Maps.Where(m => m.Activo).OrderBy(m => m.Nombre), "Id", "Nombre", mapId);
            ViewBag.SurvivorList = new SelectList(_context.Survivors.Where(s => s.Activo).OrderBy(s => s.Nombre), "Id", "Nombre", survivorId);

            ViewData["pagina"] = pagina;
            ViewData["tamano"] = tamano;
            ViewData["total"] = total;
            ViewData["killerId"] = killerId;
            ViewData["mapId"] = mapId;
            ViewData["survivorId"] = survivorId;

            return View(matches);
        }

        // GET: Matches/GetDetails/5 (devuelve HTML parcial para el modal)
        [HttpGet]
        public async Task<IActionResult> GetDetails(int id)
        {
            var match = await _context.Matches
                .Include(m => m.Killer)
                    .ThenInclude(k => k.Perks!.Where(p => p.Activo))
                .Include(m => m.Killer)
                    .ThenInclude(k => k.Addons!.Where(a => a.Activo))
                .Include(m => m.Survivor1).ThenInclude(s => s.Perks!.Where(p => p.Activo))
                .Include(m => m.Survivor2).ThenInclude(s => s.Perks!.Where(p => p.Activo))
                .Include(m => m.Survivor3).ThenInclude(s => s.Perks!.Where(p => p.Activo))
                .Include(m => m.Survivor4).ThenInclude(s => s.Perks!.Where(p => p.Activo))
                .Include(m => m.Map)
                .FirstOrDefaultAsync(m => m.Id == id && m.Activo == true);

            if (match == null) return NotFound();

            return PartialView("_MatchDetailsModal", match);
        }
    }
}