using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tp_AppVet.Data;
using Tp_AppVet.Models;

namespace Tp_AppVet.Controllers
{
    [Authorize(Roles = "Administrador,Pendiente")]
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Clientes.Include(c => c.Usuario);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            Console.WriteLine("Entró al POST de Create");
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Email");
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellido,Dni,Telefono,UsuarioId")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();

                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Id == cliente.UsuarioId);

                if(usuario != null)
                {
                    usuario.Rol = "Cliente";
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario.Email),
                        new Claim(ClaimTypes.Email, usuario.Email),
                        new Claim(ClaimTypes.Role, usuario.Rol)
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                }

                TempData["SuccessMessage"] = $"Cliente creado Exitosamente: {usuario?.Email}";
                return RedirectToAction("Index","ClienteDashboard");
            }

            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Email", cliente.UsuarioId);
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Email", cliente.UsuarioId);
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,Dni,Telefono,UsuarioId")] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
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
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Email", cliente.UsuarioId);
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Usuario)
                .Include(c => c.Mascotas)
                .Include(c => c.Turnos)
                .FirstOrDefaultAsync(c => c.Id == id);
            
            if(cliente == null)
            {
                return NotFound();
            }
            
            if ((cliente.Mascotas != null && cliente.Mascotas.Any()) 
                || (cliente.Turnos != null && cliente.Turnos.Any()))
            {
                TempData["ErrorMessage"] = "No se puede eliminar este cliente porque tiene mascotas o turnos asignados.";
                return RedirectToAction(nameof(Index));
            }

            //Eliminar cliente
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            //Eliminar Usuario asociado
            if (cliente.Usuario != null)
            {
                _context.Usuarios.Remove(cliente.Usuario);
                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] = "Cliente eliminado correctamente";
            return RedirectToAction("Index", "ClienteDashboard");
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}
