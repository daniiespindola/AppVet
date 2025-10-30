using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Tp_AppVet.Data;

namespace Tp_AppVet.Controllers
{
    [Authorize(Roles = "Administrador,Cliente")]
    public class ClienteDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ClienteDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
                return NotFound("Usuario no encontrado");

            var cliente = await _context.Clientes
                .Include(c => c.Mascotas)
                    .ThenInclude(m => m.Turnos)
                .Include(c => c.Mascotas)
                    .ThenInclude(m => m.FichaMedica)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuario.Id);

            if (cliente == null)
                return View("Cliente No Encontrado");

            return View(cliente);
        }
    }
}
