using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    public class UsuariosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        
    }
}
