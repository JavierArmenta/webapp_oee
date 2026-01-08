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

        // ========== BOTONES ==========

        public async Task<IActionResult> Botones()
        {
            var botones = await _context.Botones
                .Include(b => b.DepartamentoOperador)
                .OrderBy(b => b.Codigo)
                .ToListAsync();
            return View(botones);
        }

        public async Task<IActionResult> CreateBoton()
        {
            ViewBag.Departamentos = new SelectList(
                await _context.DepartamentosOperador.Where(d => d.Activo).OrderBy(d => d.Nombre).ToListAsync(),
                "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBoton([Bind("Codigo,Nombre,DepartamentoOperadorId,Descripcion")] Boton boton)
        {
            if (ModelState.IsValid)
            {
                boton.Activo = true;
                boton.FechaCreacion = DateTime.UtcNow;
                _context.Add(boton);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Botón creado exitosamente.";
                return RedirectToAction(nameof(Botones));
            }

            ViewBag.Departamentos = new SelectList(
                await _context.DepartamentosOperador.Where(d => d.Activo).OrderBy(d => d.Nombre).ToListAsync(),
                "Id", "Nombre", boton.DepartamentoOperadorId);
            return View(boton);
        }

        public async Task<IActionResult> EditBoton(int? id)
        {
            if (id == null)
                return NotFound();

            var boton = await _context.Botones.FindAsync(id);
            if (boton == null)
                return NotFound();

            ViewBag.Departamentos = new SelectList(
                await _context.DepartamentosOperador.Where(d => d.Activo).OrderBy(d => d.Nombre).ToListAsync(),
                "Id", "Nombre", boton.DepartamentoOperadorId);
            return View(boton);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBoton(int id, [Bind("Id,Nombre,Codigo,DepartamentoOperadorId,Descripcion,Activo,FechaCreacion,FechaUltimaActivacion")] Boton boton)
        {
            if (id != boton.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(boton);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Botón actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BotonExists(boton.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Botones));
            }
            ViewBag.Departamentos = new SelectList(
                await _context.DepartamentosOperador.Where(d => d.Activo).OrderBy(d => d.Nombre).ToListAsync(),
                "Id", "Nombre", boton.DepartamentoOperadorId);
            return View(boton);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeactivateBoton(int id)
        {
            var boton = await _context.Botones.FindAsync(id);
            if (boton != null)
            {
                boton.Activo = false;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Botón desactivado exitosamente.";
            }
            else
            {
                TempData["Error"] = "No se pudo encontrar el botón.";
            }
            return RedirectToAction(nameof(Botones));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateBoton(int id)
        {
            var boton = await _context.Botones.FindAsync(id);
            if (boton != null)
            {
                boton.Activo = true;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Botón activado exitosamente.";
            }
            else
            {
                TempData["Error"] = "No se pudo encontrar el botón.";
            }
            return RedirectToAction(nameof(Botones));
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

        private bool BotonExists(int id)
        {
            return _context.Botones.Any(e => e.Id == id);
        }
    }
}
