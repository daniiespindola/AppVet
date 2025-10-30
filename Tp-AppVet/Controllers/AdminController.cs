using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tp_AppVet.Data;

namespace Tp_AppVet.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        //GET: /Admin/Dashboard
        public IActionResult Dashboard()
        {
            var usuarios = _context.Usuarios.ToList();
            return View(usuarios);
        }

        // Acción para cambiar rol de usuario
        [HttpPost]
        public IActionResult CambiarRol(int id) {
            var usuario = _context.Usuarios.Find(id);
            if(usuario != null)
            {
                usuario.Rol = (usuario.Rol == "Administrador") ? "Cliente" : "Administrador";
                _context.SaveChanges();
                TempData["SuccessMessage"] = usuario.Rol == "Administrador"
                ? $"{usuario.Email} ahora es Administrador."
                : $"{usuario.Email} ya no es Administrador.";
            }
            return RedirectToAction("Dashboard");
        }
    }
}
