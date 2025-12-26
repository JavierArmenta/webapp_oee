using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    public class OperadoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IOperadorService _operadorService;

        public OperadoresController(ApplicationDbContext context, IOperadorService operadorService)
        {
            _context = context;
            _operadorService = operadorService;
        }

        // GET: Operadores
        public async Task<IActionResult> Index()
        {
            var operadores = await _context.Operadores
                .OrderBy(o => o.NumeroEmpleado)
                .ToListAsync();
            return View(operadores);
        }

        // GET: Operadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operador = await _context.Operadores
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (operador == null)
            {
                return NotFound();
            }

            return View(operador);
        }

        // GET: Operadores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Operadores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Apellido,NumeroEmpleado")] Operador operador, string CodigoPin)
        {
            if (ModelState.IsValid)
            {
                // Verificar si el número de empleado ya existe
                if (await _context.Operadores.AnyAsync(o => o.NumeroEmpleado == operador.NumeroEmpleado))
                {
                    ModelState.AddModelError("NumeroEmpleado", "Este número de empleado ya existe.");
                    return View(operador);
                }

                // Validar PIN
                if (string.IsNullOrWhiteSpace(CodigoPin))
                {
                    ModelState.AddModelError("CodigoPin", "El código PIN es requerido.");
                    return View(operador);
                }

                if (CodigoPin.Length < 4)
                {
                    ModelState.AddModelError("CodigoPin", "El código PIN debe tener al menos 4 dígitos.");
                    return View(operador);
                }

                // Hashear el PIN
                operador.CodigoPinHashed = _operadorService.HashPin(CodigoPin);
                operador.FechaCreacion = DateTime.UtcNow;
                operador.Activo = true;

                _context.Add(operador);
                await _context.SaveChangesAsync();
                
                TempData["Success"] = "Operador creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(operador);
        }

        // GET: Operadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operador = await _context.Operadores.FindAsync(id);
            if (operador == null)
            {
                return NotFound();
            }
            return View(operador);
        }

        // POST: Operadores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,NumeroEmpleado,Activo")] Operador operador, string CodigoPin)
        {
            if (id != operador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var operadorExistente = await _context.Operadores.FindAsync(id);
                    if (operadorExistente == null)
                    {
                        return NotFound();
                    }

                    // Verificar si el número de empleado ya existe en otro registro
                    if (await _context.Operadores.AnyAsync(o => o.NumeroEmpleado == operador.NumeroEmpleado && o.Id != id))
                    {
                        ModelState.AddModelError("NumeroEmpleado", "Este número de empleado ya existe.");
                        return View(operador);
                    }

                    // Actualizar propiedades
                    operadorExistente.Nombre = operador.Nombre;
                    operadorExistente.Apellido = operador.Apellido;
                    operadorExistente.NumeroEmpleado = operador.NumeroEmpleado;
                    operadorExistente.Activo = operador.Activo;

                    // Solo actualizar PIN si se proporciona uno nuevo
                    if (!string.IsNullOrWhiteSpace(CodigoPin))
                    {
                        if (CodigoPin.Length < 4)
                        {
                            ModelState.AddModelError("CodigoPin", "El código PIN debe tener al menos 4 dígitos.");
                            return View(operador);
                        }
                        operadorExistente.CodigoPinHashed = _operadorService.HashPin(CodigoPin);
                    }

                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Operador actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OperadorExists(operador.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(operador);
        }

        // GET: Operadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operador = await _context.Operadores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (operador == null)
            {
                return NotFound();
            }

            return View(operador);
        }

        // POST: Operadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var operador = await _context.Operadores.FindAsync(id);
            if (operador != null)
            {
                _context.Operadores.Remove(operador);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Operador eliminado exitosamente.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool OperadorExists(int id)
        {
            return _context.Operadores.Any(e => e.Id == id);
        }
    }
}