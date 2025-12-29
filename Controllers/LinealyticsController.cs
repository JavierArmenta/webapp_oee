using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.Models.Linealytics;

namespace WebApp.Controllers
{
    [Authorize]
    public class LinealyticsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LinealyticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ========== DASHBOARD ==========

        public IActionResult Index()
        {
            return View();
        }

        // ========== TURNOS ==========

        public async Task<IActionResult> Turnos()
        {
            var turnos = await _context.Turnos
                .OrderBy(t => t.HoraInicio)
                .ToListAsync();
            return View(turnos);
        }

        public IActionResult CreateTurno()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTurno([Bind("Nombre,HoraInicio,HoraFin,DuracionMinutos")] Turno turno)
        {
            if (ModelState.IsValid)
            {
                turno.Activo = true;
                _context.Add(turno);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Turno creado exitosamente.";
                return RedirectToAction(nameof(Turnos));
            }
            return View(turno);
        }

        public async Task<IActionResult> EditTurno(int? id)
        {
            if (id == null) return NotFound();

            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null) return NotFound();

            return View(turno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTurno(int id, [Bind("Id,Nombre,HoraInicio,HoraFin,DuracionMinutos,Activo")] Turno turno)
        {
            if (id != turno.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(turno);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Turno actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TurnoExists(turno.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Turnos));
            }
            return View(turno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeactivateTurno(int id)
        {
            var turno = await _context.Turnos.FindAsync(id);
            if (turno != null)
            {
                turno.Activo = false;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Turno desactivado exitosamente.";
            }
            else
            {
                TempData["Error"] = "No se pudo encontrar el turno.";
            }
            return RedirectToAction(nameof(Turnos));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateTurno(int id)
        {
            var turno = await _context.Turnos.FindAsync(id);
            if (turno != null)
            {
                turno.Activo = true;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Turno activado exitosamente.";
            }
            else
            {
                TempData["Error"] = "No se pudo encontrar el turno.";
            }
            return RedirectToAction(nameof(Turnos));
        }

        // ========== PRODUCTOS ==========

        public async Task<IActionResult> Productos()
        {
            var productos = await _context.Productos
                .OrderBy(p => p.Nombre)
                .ToListAsync();
            return View(productos);
        }

        public IActionResult CreateProducto()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProducto([Bind("Codigo,Nombre,Descripcion,TiempoCicloSegundos,UnidadesPorCiclo")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                producto.Activo = true;
                _context.Add(producto);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Producto creado exitosamente.";
                return RedirectToAction(nameof(Productos));
            }
            return View(producto);
        }

        public async Task<IActionResult> EditProducto(int? id)
        {
            if (id == null) return NotFound();

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProducto(int id, [Bind("Id,Codigo,Nombre,Descripcion,TiempoCicloSegundos,UnidadesPorCiclo,Activo")] Producto producto)
        {
            if (id != producto.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Producto actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Productos));
            }
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeactivateProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                producto.Activo = false;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Producto desactivado exitosamente.";
            }
            else
            {
                TempData["Error"] = "No se pudo encontrar el producto.";
            }
            return RedirectToAction(nameof(Productos));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                producto.Activo = true;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Producto activado exitosamente.";
            }
            else
            {
                TempData["Error"] = "No se pudo encontrar el producto.";
            }
            return RedirectToAction(nameof(Productos));
        }

        // ========== CATEGORÍAS DE PARO ==========

        public async Task<IActionResult> CategoriasParo()
        {
            var categorias = await _context.CategoriasParo
                .Include(c => c.CausasParo)
                .OrderBy(c => c.Nombre)
                .ToListAsync();
            return View(categorias);
        }

        public IActionResult CreateCategoriaParo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategoriaParo([Bind("Nombre,Descripcion,Color,EsPlaneado")] CategoriaParo categoria)
        {
            if (ModelState.IsValid)
            {
                categoria.Activo = true;
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Categoría de paro creada exitosamente.";
                return RedirectToAction(nameof(CategoriasParo));
            }
            return View(categoria);
        }

        public async Task<IActionResult> EditCategoriaParo(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.CategoriasParo.FindAsync(id);
            if (categoria == null) return NotFound();

            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategoriaParo(int id, [Bind("Id,Nombre,Descripcion,Color,EsPlaneado,Activo")] CategoriaParo categoria)
        {
            if (id != categoria.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Categoría de paro actualizada exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaParoExists(categoria.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(CategoriasParo));
            }
            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeactivateCategoriaParo(int id)
        {
            var categoria = await _context.CategoriasParo.FindAsync(id);
            if (categoria != null)
            {
                categoria.Activo = false;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Categoría de paro desactivada exitosamente.";
            }
            else
            {
                TempData["Error"] = "No se pudo encontrar la categoría de paro.";
            }
            return RedirectToAction(nameof(CategoriasParo));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateCategoriaParo(int id)
        {
            var categoria = await _context.CategoriasParo.FindAsync(id);
            if (categoria != null)
            {
                categoria.Activo = true;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Categoría de paro activada exitosamente.";
            }
            else
            {
                TempData["Error"] = "No se pudo encontrar la categoría de paro.";
            }
            return RedirectToAction(nameof(CategoriasParo));
        }

        // ========== CAUSAS DE PARO ==========

        public async Task<IActionResult> CausasParo()
        {
            var causas = await _context.CausasParo
                .Include(c => c.CategoriaParo)
                .OrderBy(c => c.CategoriaParo.Nombre)
                .ThenBy(c => c.Nombre)
                .ToListAsync();
            return View(causas);
        }

        public async Task<IActionResult> CreateCausaParo()
        {
            ViewBag.Categorias = new SelectList(
                await _context.CategoriasParo.Where(c => c.Activo).ToListAsync(),
                "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCausaParo([Bind("CategoriaParoId,Nombre,Descripcion,CodigoInterno,RequiereMantenimiento")] CausaParo causa)
        {
            if (ModelState.IsValid)
            {
                causa.Activo = true;
                _context.Add(causa);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Causa de paro creada exitosamente.";
                return RedirectToAction(nameof(CausasParo));
            }
            ViewBag.Categorias = new SelectList(
                await _context.CategoriasParo.Where(c => c.Activo).ToListAsync(),
                "Id", "Nombre", causa.CategoriaParoId);
            return View(causa);
        }

        public async Task<IActionResult> EditCausaParo(int? id)
        {
            if (id == null) return NotFound();

            var causa = await _context.CausasParo.FindAsync(id);
            if (causa == null) return NotFound();

            ViewBag.Categorias = new SelectList(
                await _context.CategoriasParo.Where(c => c.Activo).ToListAsync(),
                "Id", "Nombre", causa.CategoriaParoId);
            return View(causa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCausaParo(int id, [Bind("Id,CategoriaParoId,Nombre,Descripcion,CodigoInterno,RequiereMantenimiento,Activo")] CausaParo causa)
        {
            if (id != causa.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(causa);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Causa de paro actualizada exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CausaParoExists(causa.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(CausasParo));
            }
            ViewBag.Categorias = new SelectList(
                await _context.CategoriasParo.Where(c => c.Activo).ToListAsync(),
                "Id", "Nombre", causa.CategoriaParoId);
            return View(causa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeactivateCausaParo(int id)
        {
            var causa = await _context.CausasParo.FindAsync(id);
            if (causa != null)
            {
                causa.Activo = false;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Causa de paro desactivada exitosamente.";
            }
            else
            {
                TempData["Error"] = "No se pudo encontrar la causa de paro.";
            }
            return RedirectToAction(nameof(CausasParo));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateCausaParo(int id)
        {
            var causa = await _context.CausasParo.FindAsync(id);
            if (causa != null)
            {
                causa.Activo = true;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Causa de paro activada exitosamente.";
            }
            else
            {
                TempData["Error"] = "No se pudo encontrar la causa de paro.";
            }
            return RedirectToAction(nameof(CausasParo));
        }

        // ========== MÉTODOS AUXILIARES ==========

        private bool TurnoExists(int id)
        {
            return _context.Turnos.Any(e => e.Id == id);
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }

        private bool CategoriaParoExists(int id)
        {
            return _context.CategoriasParo.Any(e => e.Id == id);
        }

        private bool CausaParoExists(int id)
        {
            return _context.CausasParo.Any(e => e.Id == id);
        }
    }
}
