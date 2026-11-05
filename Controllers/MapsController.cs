using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DBDHub.Data;
using DBDHub.Models;

namespace DBDHub.Controllers
{
    public class MapsController : Controller
    {
        private readonly AppDbContext _context;

        public MapsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Maps
        public async Task<IActionResult> Index(string buscar, string tipo, int pagina = 1, int tamano = 18)
        {
            pagina = Math.Max(1, pagina);
            tamano = Math.Clamp(tamano, 1, 100);

            var query = _context.Maps
                .Include(m => m.MapKillers).ThenInclude(mk => mk.Killer)
                .Where(m => m.Activo == true);

            if (!string.IsNullOrEmpty(buscar))
                query = query.Where(m => m.Nombre.Contains(buscar));

            if (!string.IsNullOrEmpty(tipo))
                query = query.Where(m => m.Tipo == tipo);

            int total = await query.CountAsync();
            var maps = await query.OrderBy(m => m.Nombre)
                .Skip((pagina - 1) * tamano).Take(tamano).ToListAsync();

            ViewData["pagina"] = pagina;
            ViewData["tamano"] = tamano;
            ViewData["total"] = total;
            ViewData["buscar"] = buscar;
            ViewData["tipo"] = tipo;

            return View(maps);
        }

        // GET: Maps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var map = await _context.Maps
                .Include(m => m.MapKillers).ThenInclude(mk => mk.Killer)
                .Include(m => m.Variantes)
                .FirstOrDefaultAsync(m => m.Id == id && m.Activo);
            if (map == null) return NotFound();
            return View(map);
        }

        // GET: Maps/Create
        public IActionResult Create()
        {
            ViewData["OriginalMapId"] = new SelectList(_context.Maps.Where(m => m.Activo && m.Tipo == "Original"), "Id", "Nombre");
            return View();
        }

        // POST: Maps/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,ImagenUrl,Tipo,OriginalMapId")] Map map)
        {
            if (ModelState.IsValid)
            {
                map.Activo = true;
                map.FechaCreacion = DateTime.UtcNow;
                _context.Add(map);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OriginalMapId"] = new SelectList(_context.Maps.Where(m => m.Activo && m.Tipo == "Original"), "Id", "Nombre", map.OriginalMapId);
            return View(map);
        }

        // GET: Maps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var map = await _context.Maps.FindAsync(id);
            if (map == null || !map.Activo) return NotFound();
            ViewData["OriginalMapId"] = new SelectList(_context.Maps.Where(m => m.Activo && m.Tipo == "Original" && m.Id != id), "Id", "Nombre", map.OriginalMapId);
            return View(map);
        }

        // POST: Maps/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Nombre,ImagenUrl,Tipo,OriginalMapId,Activo")] Map map)
        {
            map.Id = id;

            if (ModelState.IsValid)
            {
                try
                {
                    var original = await _context.Maps.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
                    map.FechaCreacion = original?.FechaCreacion ?? DateTime.UtcNow;
                    _context.Update(map);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MapExists(map.Id)) return NotFound(); else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["OriginalMapId"] = new SelectList(_context.Maps.Where(m => m.Activo && m.Tipo == "Original" && m.Id != id), "Id", "Nombre", map.OriginalMapId);
            return View(map);
        }

        // GET: Maps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var map = await _context.Maps.FirstOrDefaultAsync(m => m.Id == id && m.Activo);
            if (map == null) return NotFound();
            return View(map);
        }

        // POST: Maps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var map = await _context.Maps.FindAsync(id);
            if (map != null)
            {
                map.Activo = false;
                _context.Update(map);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool MapExists(int id) => _context.Maps.Any(e => e.Id == id);
    }
}