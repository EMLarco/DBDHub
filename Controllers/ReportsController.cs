using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DBDHub.Data;

namespace DBDHub.Controllers
{
    public class ReportsController : Controller
    {
        private readonly AppDbContext _context;

        public ReportsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Reports
        public IActionResult Index()
        {
            return View();
        }

        // 1. Total de partidas (Count)
        public async Task<IActionResult> TotalMatches()
        {
            var total = await _context.Matches
                .AsNoTracking()
                .Where(m => m.Activo)
                .CountAsync();

            ViewBag.TotalMatches = total;
            return View();
        }

        // 2. Promedio de puntuación total (Average)
        public async Task<IActionResult> AverageScore()
        {
            var avg = await _context.Matches
                .AsNoTracking()
                .Where(m => m.Activo)
                .AverageAsync(m => m.TotalScore);

            ViewBag.AverageScore = avg.ToString("N2");
            return View();
        }

        // 3. Suma de puntuaciones secundarias (Sum)
        public async Task<IActionResult> TotalDetails()
        {
            var sum1 = await _context.Matches
                .AsNoTracking()
                .Where(m => m.Activo)
                .SumAsync(m => m.Detail1);
            var sum2 = await _context.Matches
                .AsNoTracking()
                .Where(m => m.Activo)
                .SumAsync(m => m.Detail2);
            var sum3 = await _context.Matches
                .AsNoTracking()
                .Where(m => m.Activo)
                .SumAsync(m => m.Detail3);
            var sum4 = await _context.Matches
                .AsNoTracking()
                .Where(m => m.Activo)
                .SumAsync(m => m.Detail4);

            ViewBag.TotalDetail1 = sum1.ToString("N0");
            ViewBag.TotalDetail2 = sum2.ToString("N0");
            ViewBag.TotalDetail3 = sum3.ToString("N0");
            ViewBag.TotalDetail4 = sum4.ToString("N0");
            return View();
        }

        // 4. Partidas agrupadas por mapa (GroupBy, Count, Average)
        public async Task<IActionResult> MapStats()
        {
            var stats = await _context.Matches
                .AsNoTracking()
                .Where(m => m.Activo)
                .GroupBy(m => m.MapName)
                .Select(g => new
                {
                    Map = g.Key,
                    Count = g.Count(),
                    AvgScore = g.Average(m => m.TotalScore)
                })
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            return View(stats);
        }

        // 5. Partidas por resultado (GroupBy, Count)
        public async Task<IActionResult> ResultDistribution()
        {
            var distribution = await _context.Matches
                .AsNoTracking()
                .Where(m => m.Activo)
                .GroupBy(m => m.Result)
                .Select(g => new
                {
                    Result = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            return View(distribution);
        }

        // 6. Top 10 asesinos más usados (GroupBy, Count, OrderByDescending, Take)
        public async Task<IActionResult> TopKillers()
        {
            var top = await _context.Matches
                .AsNoTracking()
                .Where(m => m.Activo && m.KillerId != null)
                .GroupBy(m => m.Killer!.Nombre)
                .Select(g => new
                {
                    Killer = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(10)
                .ToListAsync();

            return View(top);
        }

        // 7. Top 10 supervivientes más usados (GroupBy, Count, OrderByDescending, Take)
        // Se unifican los cuatro campos de supervivientes usando UNION simulada con Concat
        public async Task<IActionResult> TopSurvivors()
        {
            var survivors1 = _context.Matches
                .AsNoTracking()
                .Where(m => m.Activo && m.Survivor1Id != null)
                .Select(m => m.Survivor1!.Nombre);

            var survivors2 = _context.Matches
                .AsNoTracking()
                .Where(m => m.Activo && m.Survivor2Id != null)
                .Select(m => m.Survivor2!.Nombre);

            var survivors3 = _context.Matches
                .AsNoTracking()
                .Where(m => m.Activo && m.Survivor3Id != null)
                .Select(m => m.Survivor3!.Nombre);

            var survivors4 = _context.Matches
                .AsNoTracking()
                .Where(m => m.Activo && m.Survivor4Id != null)
                .Select(m => m.Survivor4!.Nombre);

            var allSurvivors = survivors1
                .Concat(survivors2)
                .Concat(survivors3)
                .Concat(survivors4);

            var top = await allSurvivors
                .GroupBy(name => name)
                .Select(g => new
                {
                    Survivor = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(10)
                .ToListAsync();

            return View(top);
        }

        // 8. Búsqueda de partidas por nombre de mapa (Contains / ILike)
        public async Task<IActionResult> SearchByMap(string? mapName)
        {
            if (string.IsNullOrWhiteSpace(mapName))
            {
                ViewBag.Results = null;
                return View();
            }

            var matches = await _context.Matches
                .AsNoTracking()
                .Where(m => m.Activo && EF.Functions.ILike(m.MapName, $"%{mapName}%"))
                .OrderByDescending(m => m.FechaPartida)
                .Take(50)
                .ToListAsync();

            ViewBag.MapName = mapName;
            ViewBag.Results = matches;
            return View();
        }
    }
}