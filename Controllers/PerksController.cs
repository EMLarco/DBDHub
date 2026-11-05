using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DBDHub.Data;
using DBDHub.Models;

namespace DBDHub.Controllers
{
    public class PerksController : Controller
    {
        private readonly AppDbContext _context;

        public PerksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Perks (con paginación y búsqueda)
        // GET: Perks
        public async Task<IActionResult> Index(string buscar, string tipo, int pagina = 1, int tamano = 18)
        {
            pagina = Math.Max(1, pagina);
            tamano = Math.Clamp(tamano, 1, 100);

            var query = _context.Perks
                .Include(p => p.Killer)
                .Include(p => p.Survivor)
                .Where(p => p.Activo == true);

            if (!string.IsNullOrEmpty(buscar))
                query = query.Where(p => p.Nombre.Contains(buscar));

            if (!string.IsNullOrEmpty(tipo))
                query = query.Where(p => p.TipoPersonaje == tipo);

            int totalRegistros = await query.CountAsync();
            var perks = await query
                .OrderBy(p => p.Nombre)
                .Skip((pagina - 1) * tamano)
                .Take(tamano)
                .ToListAsync();

            ViewData["pagina"] = pagina;
            ViewData["tamano"] = tamano;
            ViewData["totalRegistros"] = totalRegistros;
            ViewData["buscar"] = buscar;
            ViewData["tipo"] = tipo;

            return View(perks);
        }

        // GET: Perks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var perk = await _context.Perks
                .Include(p => p.Killer)
                .Include(p => p.Survivor)
                .FirstOrDefaultAsync(m => m.Id == id && m.Activo == true);
            if (perk == null) return NotFound();

            return View(perk);
        }

        // GET: Perks/Create
        public IActionResult Create()
        {
            ViewData["KillerId"] = new SelectList(_context.Killers.Where(k => k.Activo), "Id", "Nombre");
            ViewData["SurvivorId"] = new SelectList(_context.Survivors.Where(s => s.Activo), "Id", "Nombre");
            return View();
        }

        // POST: Perks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Descripcion,Nivel,TipoPersonaje,KillerId,SurvivorId,ImagenUrl")] Perk perk)
        {
            if (ModelState.IsValid)
            {
                perk.Activo = true;
                perk.FechaCreacion = DateTime.UtcNow;
                _context.Add(perk);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KillerId"] = new SelectList(_context.Killers.Where(k => k.Activo), "Id", "Nombre", perk.KillerId);
            ViewData["SurvivorId"] = new SelectList(_context.Survivors.Where(s => s.Activo), "Id", "Nombre", perk.SurvivorId);
            return View(perk);
        }

        // GET: Perks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var perk = await _context.Perks.FindAsync(id);
            if (perk == null || perk.Activo == false) return NotFound();

            ViewData["KillerId"] = new SelectList(_context.Killers.Where(k => k.Activo), "Id", "Nombre", perk.KillerId);
            ViewData["SurvivorId"] = new SelectList(_context.Survivors.Where(s => s.Activo), "Id", "Nombre", perk.SurvivorId);
            return View(perk);
        }

        // POST: Perks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Nombre,Descripcion,Nivel,TipoPersonaje,KillerId,SurvivorId,Activo,ImagenUrl")] Perk perk)
        {
            perk.Id = id;

            if (ModelState.IsValid)
            {
                try
                {
                    var original = await _context.Perks.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                    perk.FechaCreacion = original?.FechaCreacion ?? DateTime.UtcNow;
                    _context.Update(perk);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PerkExists(perk.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["KillerId"] = new SelectList(_context.Killers.Where(k => k.Activo), "Id", "Nombre", perk.KillerId);
            ViewData["SurvivorId"] = new SelectList(_context.Survivors.Where(s => s.Activo), "Id", "Nombre", perk.SurvivorId);
            return View(perk);
        }

        // GET: Perks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var perk = await _context.Perks
                .Include(p => p.Killer)
                .Include(p => p.Survivor)
                .FirstOrDefaultAsync(m => m.Id == id && m.Activo == true);
            if (perk == null) return NotFound();
            return View(perk);
        }

        // POST: Perks/Delete/5 (eliminación lógica)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var perk = await _context.Perks.FindAsync(id);
            if (perk != null)
            {
                perk.Activo = false;
                _context.Update(perk);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PerkExists(int id) => _context.Perks.Any(e => e.Id == id);
    }
}