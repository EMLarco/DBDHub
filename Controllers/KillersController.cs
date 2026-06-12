using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DBDHub.Data;
using DBDHub.Models;

namespace DBDHub.Controllers
{
    public class KillersController : Controller
    {
        private readonly AppDbContext _context;

        public KillersController(AppDbContext context)
        {
            _context = context;
        }

 // GET: Killers
public async Task<IActionResult> Index(string buscar, int pagina = 1, int tamano = 10)
{
    // Consulta base solo activos
    var query = _context.Killers.Where(k => k.Activo == true);

    // Filtro de búsqueda
    if (!string.IsNullOrEmpty(buscar))
    {
        query = query.Where(k => k.Nombre.Contains(buscar));
    }

    // Total de registros para calcular páginas
    int totalRegistros = await query.CountAsync();

    // Aplicar paginación
    var killers = await query
        .OrderBy(k => k.Nombre)
        .Skip((pagina - 1) * tamano)
        .Take(tamano)
        .ToListAsync();

    // Pasar datos a la vista
    ViewData["pagina"] = pagina;
    ViewData["tamano"] = tamano;
    ViewData["totalRegistros"] = totalRegistros;
    ViewData["buscar"] = buscar;

    return View(killers);
}

        // GET: Killers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var killer = await _context.Killers
                .Include(k => k.Perks!.Where(p => p.Activo))
                .Include(k => k.Addons!.Where(a => a.Activo))
                .Include(k => k.MapKillers!)
                    .ThenInclude(mk => mk.Map)
                .FirstOrDefaultAsync(m => m.Id == id && m.Activo == true);

            if (killer == null) return NotFound();

            return View(killer);
        }

        // GET: Killers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Killers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Poder,Velocidad,Dificultad,Historia")] Killer killer)
        {
            if (ModelState.IsValid)
            {
                killer.Activo = true;
                killer.FechaCreacion = DateTime.UtcNow;
                _context.Add(killer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(killer);
        }

        // GET: Killers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var killer = await _context.Killers.FindAsync(id);
            if (killer == null || killer.Activo == false) return NotFound();

            return View(killer);
        }

        // POST: Killers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Poder,Velocidad,Dificultad,Historia,Activo")] Killer killer)
        {
            if (id != killer.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Mantener la fecha de creación original
                    var killerOriginal = await _context.Killers.AsNoTracking().FirstOrDefaultAsync(k => k.Id == id);
                    killer.FechaCreacion = killerOriginal?.FechaCreacion ?? DateTime.UtcNow;

                    _context.Update(killer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KillerExists(killer.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(killer);
        }

        // GET: Killers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var killer = await _context.Killers
                .FirstOrDefaultAsync(m => m.Id == id && m.Activo == true);
            if (killer == null) return NotFound();

            return View(killer);
        }

        // POST: Killers/Delete/5 (eliminación lógica)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var killer = await _context.Killers.FindAsync(id);
            if (killer != null)
            {
                killer.Activo = false; // eliminación lógica
                _context.Update(killer);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool KillerExists(int id)
        {
            return _context.Killers.Any(e => e.Id == id);
        }
    }
}