using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DBDHub.Data;
using DBDHub.Models;
using DBDHub.Services;

namespace DBDHub.Controllers
{
    public class MatchesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILiveEventPublisher _live;

        public MatchesController(AppDbContext context, ILiveEventPublisher live)
        {
            _context = context;
            _live = live;
        }

        // GET: Matches (con filtros y paginación)
        public async Task<IActionResult> Index(int? killerId, int? mapId, int? survivorId, int pagina = 1, int tamano = 5)
        {
            pagina = Math.Max(1, pagina);
            tamano = Math.Clamp(tamano, 1, 100);

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

        // GET: Matches/Create
        [HttpGet]
        public IActionResult Create()
        {
            PopulateMatchLists();
            return View(new Match
            {
                FechaPartida = DateTime.UtcNow,
                Result = "Merciless Killer",
                DuracionSegundos = 600,
                TotalScore = 50000
            });
        }

        // POST: Matches/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("FechaPartida,KillerId,MapId,Survivor1Id,Survivor2Id,Survivor3Id,Survivor4Id,TotalScore,Detail1,Detail2,Detail3,Detail4,Result,DuracionSegundos")] Match match)
        {
            var survivorIds = new[] { match.Survivor1Id, match.Survivor2Id, match.Survivor3Id, match.Survivor4Id }
                .Where(id => id.HasValue)
                .Select(id => id!.Value)
                .ToList();

            if (survivorIds.Count != 4 || survivorIds.Distinct().Count() != 4)
            {
                ModelState.AddModelError(string.Empty, "Selecciona 4 supervivientes distintos.");
            }

            var killer = match.KillerId.HasValue
                ? await _context.Killers.FirstOrDefaultAsync(k => k.Id == match.KillerId && k.Activo)
                : null;
            if (killer == null)
            {
                ModelState.AddModelError(nameof(match.KillerId), "Selecciona un killer valido.");
            }

            var map = match.MapId.HasValue
                ? await _context.Maps.FirstOrDefaultAsync(m => m.Id == match.MapId && m.Activo)
                : null;
            if (map == null)
            {
                ModelState.AddModelError(nameof(match.MapId), "Selecciona un mapa valido.");
            }

            if (survivorIds.Count == 4)
            {
                var validSurvivors = await _context.Survivors
                    .CountAsync(s => survivorIds.Contains(s.Id) && s.Activo);
                if (validSurvivors != 4)
                {
                    ModelState.AddModelError(string.Empty, "Alguno de los supervivientes no es valido.");
                }
            }

            if (!ModelState.IsValid)
            {
                PopulateMatchLists(match);
                return View(match);
            }

            match.FechaPartida = DateTime.SpecifyKind(match.FechaPartida, DateTimeKind.Utc);
            match.MapName = map!.Nombre;
            match.Location = "Reino de la Entidad";
            match.Activo = true;
            match.FechaCreacion = DateTime.UtcNow;

            _context.Matches.Add(match);
            await _context.SaveChangesAsync();

            await _live.PublishAsync(
                "match.created",
                $"Nueva partida registrada: {killer!.Nombre} en {map.Nombre}",
                new
                {
                    match.Id,
                    match.TotalScore,
                    match.Result,
                    match.DuracionSegundos,
                    match.FechaPartida,
                    Killer = killer.Nombre,
                    Map = map.Nombre
                });

            return RedirectToAction(nameof(Index));
        }

        // POST: Matches/Simulate (genera una partida aleatoria y la emite en vivo)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Simulate()
        {
            var killers = await _context.Killers.Where(k => k.Activo).ToListAsync();
            var maps = await _context.Maps.Where(m => m.Activo).ToListAsync();
            var survivors = await _context.Survivors.Where(s => s.Activo).ToListAsync();

            if (killers.Count == 0 || maps.Count == 0 || survivors.Count < 4)
            {
                return BadRequest(new { ok = false, message = "No hay datos suficientes para simular una partida." });
            }

            var random = Random.Shared;
            var killer = killers[random.Next(killers.Count)];
            var map = maps[random.Next(maps.Count)];
            var picked = survivors.OrderBy(_ => Guid.NewGuid()).Take(4).ToList();
            var results = new[] { "Merciless Killer", "Ruthless Killer", "Brutal Killer", "Dead" };

            var match = new Match
            {
                FechaPartida = DateTime.UtcNow,
                KillerId = killer.Id,
                MapId = map.Id,
                Survivor1Id = picked[0].Id,
                Survivor2Id = picked[1].Id,
                Survivor3Id = picked[2].Id,
                Survivor4Id = picked[3].Id,
                TotalScore = random.Next(20000, 100000),
                MapName = map.Nombre,
                Location = "Reino de la Entidad",
                Detail1 = random.Next(1000, 10000),
                Detail2 = random.Next(1000, 10000),
                Detail3 = random.Next(1000, 10000),
                Detail4 = random.Next(1000, 10000),
                Result = results[random.Next(results.Length)],
                DuracionSegundos = random.Next(300, 1800),
                Activo = true,
                FechaCreacion = DateTime.UtcNow
            };

            _context.Matches.Add(match);
            await _context.SaveChangesAsync();

            await _live.PublishAsync(
                "match.simulated",
                $"Partida simulada: {killer.Nombre} en {map.Nombre}",
                new
                {
                    match.Id,
                    match.TotalScore,
                    match.Result,
                    match.DuracionSegundos,
                    match.FechaPartida,
                    Killer = killer.Nombre,
                    Map = map.Nombre,
                    Survivors = picked.Select(s => s.Nombre).ToArray()
                });

            return Json(new { ok = true, id = match.Id });
        }

        // GET: Matches/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var match = await MatchDetailsQuery()
                .FirstOrDefaultAsync(m => m.Id == id && m.Activo == true);

            if (match == null) return NotFound();

            return View(match);
        }

        // GET: Matches/GetDetails/5 (devuelve HTML parcial para el modal)
        [HttpGet]
        public async Task<IActionResult> GetDetails(int id)
        {
            var match = await MatchDetailsQuery()
                .FirstOrDefaultAsync(m => m.Id == id && m.Activo == true);

            if (match == null) return NotFound();

            return PartialView("_MatchDetailsModal", match);
        }

        private void PopulateMatchLists(Match? match = null)
        {
            ViewBag.KillerList = new SelectList(
                _context.Killers.Where(k => k.Activo).OrderBy(k => k.Nombre),
                "Id", "Nombre", match?.KillerId);
            ViewBag.MapList = new SelectList(
                _context.Maps.Where(m => m.Activo).OrderBy(m => m.Nombre),
                "Id", "Nombre", match?.MapId);
            ViewBag.SurvivorList = new SelectList(
                _context.Survivors.Where(s => s.Activo).OrderBy(s => s.Nombre),
                "Id", "Nombre");
        }

        private IQueryable<Match> MatchDetailsQuery()
        {
            return _context.Matches
                .Include(m => m.Killer)
                    .ThenInclude(k => k!.Perks.Where(p => p.Activo))
                .Include(m => m.Killer)
                    .ThenInclude(k => k!.Addons.Where(a => a.Activo))
                .Include(m => m.Survivor1).ThenInclude(s => s!.Perks.Where(p => p.Activo))
                .Include(m => m.Survivor2).ThenInclude(s => s!.Perks.Where(p => p.Activo))
                .Include(m => m.Survivor3).ThenInclude(s => s!.Perks.Where(p => p.Activo))
                .Include(m => m.Survivor4).ThenInclude(s => s!.Perks.Where(p => p.Activo))
                .Include(m => m.Map);
        }
    }
}
