using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public OperadoresController(ApplicationDbContext context, IOperadorService operadorService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _operadorService = operadorService;
            _userManager = userManager;
        }

        // GET: Operadores
        public async Task<IActionResult> Index()
        {
            var operadores = await _context.Operadores
                .Include(o => o.OperadorDepartamentos)
                    .ThenInclude(od => od.DepartamentoOperador)
                .OrderBy(o => o.NumeroEmpleado)
                .ToListAsync();
            return View(operadores);
        }

        // GET: Operadores/Lista
        public async Task<IActionResult> Lista()
        {
            var operadores = await _context.Operadores
                .Include(o => o.OperadorDepartamentos)
                    .ThenInclude(od => od.DepartamentoOperador)
                .OrderBy(o => o.Id)
                .ToListAsync();
            return View(operadores);
        }

        // GET: Operadores/Usuarios
        public async Task<IActionResult> Usuarios()
        {
            var usuarios = await _userManager.Users
                .OrderBy(u => u.Email)
                .ToListAsync();
            return View(usuarios);
        }

        // GET: Operadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operador = await _context.Operadores
                .Include(o => o.OperadorDepartamentos)
                    .ThenInclude(od => od.DepartamentoOperador)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (operador == null)
            {
                return NotFound();
            }

            return View(operador);
        }

        // GET: Operadores/Departamentos/5
        public async Task<IActionResult> Departamentos(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operador = await _context.Operadores
                .Include(o => o.OperadorDepartamentos)
                    .ThenInclude(od => od.DepartamentoOperador)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (operador == null)
            {
                return NotFound();
            }

            return View(operador);
        }

        // GET: Operadores/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.DepartamentosOperador = await _context.DepartamentosOperador
                .Where(d => d.Activo)
                .OrderBy(d => d.Nombre)
                .ToListAsync();
            return View();
        }

        // POST: Operadores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Apellido,NumeroEmpleado")] Operador operador, string CodigoPin, List<int>? DepartamentosSeleccionados)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Operadores.AnyAsync(o => o.NumeroEmpleado == operador.NumeroEmpleado))
                {
                    ModelState.AddModelError("NumeroEmpleado", "Este número de empleado ya existe.");
                    ViewBag.DepartamentosOperador = await _context.DepartamentosOperador.Where(d => d.Activo).ToListAsync();
                    return View(operador);
                }

                if (string.IsNullOrWhiteSpace(CodigoPin))
                {
                    ModelState.AddModelError("CodigoPin", "El código PIN es requerido.");
                    ViewBag.DepartamentosOperador = await _context.DepartamentosOperador.Where(d => d.Activo).ToListAsync();
                    return View(operador);
                }

                if (CodigoPin.Length < 4)
                {
                    ModelState.AddModelError("CodigoPin", "El código PIN debe tener al menos 4 dígitos.");
                    ViewBag.DepartamentosOperador = await _context.DepartamentosOperador.Where(d => d.Activo).ToListAsync();
                    return View(operador);
                }

                operador.CodigoPinHashed = _operadorService.HashPin(CodigoPin);
                operador.FechaCreacion = DateTime.UtcNow;
                operador.Activo = true;

                _context.Add(operador);
                await _context.SaveChangesAsync();

                if (DepartamentosSeleccionados != null && DepartamentosSeleccionados.Any())
                {
                    foreach (var deptId in DepartamentosSeleccionados)
                    {
                        _context.OperadorDepartamentos.Add(new OperadorDepartamento
                        {
                            OperadorId = operador.Id,
                            DepartamentoOperadorId = deptId,
                            FechaAsignacion = DateTime.UtcNow
                        });
                    }
                    await _context.SaveChangesAsync();
                }

                TempData["Success"] = "Operador creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.DepartamentosOperador = await _context.DepartamentosOperador.Where(d => d.Activo).ToListAsync();
            return View(operador);
        }

        // GET: Operadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operador = await _context.Operadores
                .Include(o => o.OperadorDepartamentos)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (operador == null)
            {
                return NotFound();
            }

            ViewBag.DepartamentosOperador = await _context.DepartamentosOperador
                .Where(d => d.Activo)
                .OrderBy(d => d.Nombre)
                .ToListAsync();

            ViewBag.DepartamentosSeleccionados = operador.OperadorDepartamentos
                .Select(od => od.DepartamentoOperadorId)
                .ToList();

            return View(operador);
        }

        // POST: Operadores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,NumeroEmpleado,Activo")] Operador operador, string? CodigoPin, List<int>? DepartamentosSeleccionados)
        {
            if (id != operador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var operadorExistente = await _context.Operadores
                        .Include(o => o.OperadorDepartamentos)
                        .FirstOrDefaultAsync(o => o.Id == id);

                    if (operadorExistente == null)
                    {
                        return NotFound();
                    }

                    if (await _context.Operadores.AnyAsync(o => o.NumeroEmpleado == operador.NumeroEmpleado && o.Id != id))
                    {
                        ModelState.AddModelError("NumeroEmpleado", "Este número de empleado ya existe.");
                        ViewBag.DepartamentosOperador = await _context.DepartamentosOperador.Where(d => d.Activo).ToListAsync();
                        ViewBag.DepartamentosSeleccionados = DepartamentosSeleccionados ?? new List<int>();
                        return View(operador);
                    }

                    operadorExistente.Nombre = operador.Nombre;
                    operadorExistente.Apellido = operador.Apellido;
                    operadorExistente.NumeroEmpleado = operador.NumeroEmpleado;
                    operadorExistente.Activo = operador.Activo;

                    if (!string.IsNullOrWhiteSpace(CodigoPin))
                    {
                        if (CodigoPin.Length < 4)
                        {
                            ModelState.AddModelError("CodigoPin", "El código PIN debe tener al menos 4 dígitos.");
                            ViewBag.DepartamentosOperador = await _context.DepartamentosOperador.Where(d => d.Activo).ToListAsync();
                            ViewBag.DepartamentosSeleccionados = DepartamentosSeleccionados ?? new List<int>();
                            return View(operador);
                        }
                        operadorExistente.CodigoPinHashed = _operadorService.HashPin(CodigoPin);
                    }

                    _context.OperadorDepartamentos.RemoveRange(operadorExistente.OperadorDepartamentos);

                    if (DepartamentosSeleccionados != null && DepartamentosSeleccionados.Any())
                    {
                        foreach (var deptId in DepartamentosSeleccionados)
                        {
                            _context.OperadorDepartamentos.Add(new OperadorDepartamento
                            {
                                OperadorId = operadorExistente.Id,
                                DepartamentoOperadorId = deptId,
                                FechaAsignacion = DateTime.UtcNow
                            });
                        }
                    }

                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Operador actualizado exitosamente.";
                    return RedirectToAction(nameof(Index));
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
            }

            ViewBag.DepartamentosOperador = await _context.DepartamentosOperador.Where(d => d.Activo).ToListAsync();
            ViewBag.DepartamentosSeleccionados = DepartamentosSeleccionados ?? new List<int>();
            return View(operador);
        }

        // POST: Operadores/Deactivate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deactivate(int id)
        {
            var operador = await _context.Operadores.FindAsync(id);
            if (operador != null)
            {
                operador.Activo = false;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Operador desactivado exitosamente.";
            }
            else
            {
                TempData["Error"] = "No se pudo encontrar el operador.";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Operadores/Activate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(int id)
        {
            var operador = await _context.Operadores.FindAsync(id);
            if (operador != null)
            {
                operador.Activo = true;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Operador activado exitosamente.";
            }
            else
            {
                TempData["Error"] = "No se pudo encontrar el operador.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool OperadorExists(int id)
        {
            return _context.Operadores.Any(e => e.Id == id);
        }
    }
}
