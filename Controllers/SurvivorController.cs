using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DBDHub.Data;
using DBDHub.Models;

namespace DBDHub.Controllers
{
    public class SurvivorsController : Controller
    {
        private readonly AppDbContext _context;

        public SurvivorsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Survivors (con paginación, búsqueda y carga de Perks)
        public async Task<IActionResult> Index(string buscar, int pagina = 1, int tamano = 18)
        {
            pagina = Math.Max(1, pagina);
            tamano = Math.Clamp(tamano, 1, 100);

            var query = _context.Survivors
                .Include(s => s.Perks.Where(p => p.Activo))   // para las tarjetas
                .Where(s => s.Activo == true);

            if (!string.IsNullOrEmpty(buscar))
                query = query.Where(s => s.Nombre.Contains(buscar));

            int totalRegistros = await query.CountAsync();

            var survivors = await query
                .OrderBy(s => s.Nombre)
                .Skip((pagina - 1) * tamano)
                .Take(tamano)
                .ToListAsync();

            ViewData["pagina"] = pagina;
            ViewData["tamano"] = tamano;
            ViewData["totalRegistros"] = totalRegistros;
            ViewData["buscar"] = buscar;

            return View(survivors);
        }

        // GET: Survivors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var survivor = await _context.Survivors
                .Include(s => s.Perks.Where(p => p.Activo))
                .FirstOrDefaultAsync(m => m.Id == id && m.Activo == true);

            if (survivor == null) return NotFound();
            return View(survivor);
        }

        // GET: Survivors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Survivors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Historia,Dificultad,ImagenUrl")] Survivor survivor)
        {
            if (ModelState.IsValid)
            {
                survivor.Activo = true;
                survivor.FechaCreacion = DateTime.UtcNow;
                _context.Add(survivor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(survivor);
        }

        // GET: Survivors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var survivor = await _context.Survivors.FindAsync(id);
            if (survivor == null || survivor.Activo == false) return NotFound();
            return View(survivor);
        }

        // POST: Survivors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Nombre,Historia,Dificultad,Activo,ImagenUrl")] Survivor survivor)
        {
            survivor.Id = id;

            if (ModelState.IsValid)
            {
                try
                {
                    var original = await _context.Survivors.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
                    survivor.FechaCreacion = original?.FechaCreacion ?? DateTime.UtcNow;
                    _context.Update(survivor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SurvivorExists(survivor.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(survivor);
        }

        // GET: Survivors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var survivor = await _context.Survivors.FirstOrDefaultAsync(m => m.Id == id && m.Activo == true);
            if (survivor == null) return NotFound();
            return View(survivor);
        }

        // POST: Survivors/Delete/5 (eliminación lógica)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var survivor = await _context.Survivors.FindAsync(id);
            if (survivor != null)
            {
                survivor.Activo = false;
                _context.Update(survivor);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool SurvivorExists(int id) => _context.Survivors.Any(e => e.Id == id);
    }
}