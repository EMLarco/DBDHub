using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DBDHub.Data;
using DBDHub.Models;

namespace DBDHub.Controllers
{
    public class AddonsController : Controller
    {
        private readonly AppDbContext _context;

        public AddonsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Addons
        public async Task<IActionResult> Index(string buscar, int? killerId, int pagina = 1, int tamano = 18)
        {
            var query = _context.Addons
                .Include(a => a.Killer)
                .Where(a => a.Activo == true);

            if (!string.IsNullOrEmpty(buscar))
                query = query.Where(a => a.Nombre.Contains(buscar));

            if (killerId.HasValue)
                query = query.Where(a => a.KillerId == killerId);

            int total = await query.CountAsync();
            var addons = await query.OrderBy(a => a.Nombre)
                .Skip((pagina - 1) * tamano).Take(tamano).ToListAsync();

            ViewData["pagina"] = pagina;
            ViewData["tamano"] = tamano;
            ViewData["total"] = total;
            ViewData["buscar"] = buscar;
            ViewData["killerId"] = killerId;
            ViewBag.KillerId = new SelectList(_context.Killers.Where(k => k.Activo), "Id", "Nombre", killerId);

            return View(addons);
        }

        // GET: Addons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var addon = await _context.Addons
                .Include(a => a.Killer)
                .FirstOrDefaultAsync(m => m.Id == id && m.Activo == true);
            if (addon == null) return NotFound();

            return View(addon);
        }

        // GET: Addons/Create
        public IActionResult Create()
        {
            ViewBag.KillerId = new SelectList(_context.Killers.Where(k => k.Activo), "Id", "Nombre");
            return View();
        }

        // POST: Addons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Rareza,Descripcion,KillerId,ImagenUrl")] Addon addon)
        {
            if (ModelState.IsValid)
            {
                addon.Activo = true;
                addon.FechaCreacion = DateTime.UtcNow;
                _context.Add(addon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.KillerId = new SelectList(_context.Killers.Where(k => k.Activo), "Id", "Nombre", addon.KillerId);
            return View(addon);
        }

        // GET: Addons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var addon = await _context.Addons.FindAsync(id);
            if (addon == null || !addon.Activo) return NotFound();
            ViewBag.KillerId = new SelectList(_context.Killers.Where(k => k.Activo), "Id", "Nombre", addon.KillerId);
            return View(addon);
        }

        // POST: Addons/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Rareza,Descripcion,KillerId,Activo,ImagenUrl")] Addon addon)
        {
            if (id != addon.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    var original = await _context.Addons.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
                    addon.FechaCreacion = original?.FechaCreacion ?? DateTime.UtcNow;
                    _context.Update(addon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddonExists(addon.Id)) return NotFound(); else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.KillerId = new SelectList(_context.Killers.Where(k => k.Activo), "Id", "Nombre", addon.KillerId);
            return View(addon);
        }

        // GET: Addons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var addon = await _context.Addons.Include(a => a.Killer).FirstOrDefaultAsync(m => m.Id == id && m.Activo);
            if (addon == null) return NotFound();
            return View(addon);
        }

        // POST: Addons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var addon = await _context.Addons.FindAsync(id);
            if (addon != null)
            {
                addon.Activo = false;
                _context.Update(addon);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AddonExists(int id) => _context.Addons.Any(e => e.Id == id);
    }
}