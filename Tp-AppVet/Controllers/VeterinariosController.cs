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
    public class VeterinariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VeterinariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Veterinarios
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Veterinarios.Include(v => v.Usuario);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Veterinarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veterinario = await _context.Veterinarios
                .Include(v => v.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veterinario == null)
            {
                return NotFound();
            }

            return View(veterinario);
        }

        // GET: Veterinarios/Create
        public async Task<IActionResult> Create()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
            {
                TempData["ErrorMessage"] = "Usuario no encontrado.";
                return RedirectToAction("Index", "Home");
            }
            ViewData["UsuarioId"] = new SelectList(new List<Usuario> { usuario }, "Id", "Email", usuario.Id);
            return View();
        }

        // POST: Veterinarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellido,Matricula,Especialidad,UsuarioId")] Veterinario veterinario)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Id == veterinario.UsuarioId);
            
            if (usuario == null)
            {
                TempData["ErrorMessage"] = "Usuario no encontrado.";
                return RedirectToAction("Index", "Home");
            }
            if (veterinario.UsuarioId != usuario.Id)
            {
                ModelState.AddModelError("", "El correo seleccionado no corresponde al usuario actual.");
                ViewData["UsuarioId"] = new SelectList(new List<Usuario> { usuario }, "Id", "Email", usuario.Id);
                return View(veterinario);
            }
            if (ModelState.IsValid)
            {
                _context.Add(veterinario);
                await _context.SaveChangesAsync();
                    
                if(usuario != null)
                {
                    usuario.Rol = "Veterinario";
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

                TempData["SuccessMessage"] = $"Veterinario creado Exitosamente: {usuario?.Email}";
                return RedirectToAction("Index", "VeterinarioDashboard");
            }
            ViewData["UsuarioId"] = new SelectList(new List<Usuario> { usuario }, "Id", "Email", usuario.Id);
            return View(veterinario);
        }

        // GET: Veterinarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veterinario = await _context.Veterinarios.FindAsync(id);
            if (veterinario == null)
            {
                return NotFound();
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Email", veterinario.UsuarioId);
            return View(veterinario);
        }

        // POST: Veterinarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,Matricula,Especialidad,UsuarioId")] Veterinario veterinario)
        {
            if (id != veterinario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(veterinario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VeterinarioExists(veterinario.Id))
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
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Email", veterinario.UsuarioId);
            return View(veterinario);
        }

        // GET: Veterinarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veterinario = await _context.Veterinarios
                .Include(v => v.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veterinario == null)
            {
                return NotFound();
            }

            return View(veterinario);
        }

        // POST: Veterinarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var veterinario = await _context.Veterinarios
                .Include(v => v.Usuario)
                .Include(v => v.Turnos)
                .FirstOrDefaultAsync(v => v.Id == id);


            if (veterinario == null)
            {
                return NotFound();
            }

            if (veterinario.Turnos != null && veterinario.Turnos.Any())
            {
                TempData["ErrorMessage"] = "No se puede eliminar el veterinario porque tiene turnos asignados. Elimine los turnos primero.";
                return RedirectToAction(nameof(Index));
            }

            //Eliminar veterinario
            _context.Veterinarios.Remove(veterinario);
            await _context.SaveChangesAsync();

            //Eliminar Usuario asociado
            if (veterinario.Usuario != null)
            {
                _context.Usuarios.Remove(veterinario.Usuario);
                await _context.SaveChangesAsync();
            } 

            TempData["SuccessMessage"] = "Veterinario eliminado correctamente";
            return RedirectToAction("Index", "Veterinarios");
        }

        private bool VeterinarioExists(int id)
        {
            return _context.Veterinarios.Any(e => e.Id == id);
        }
    }
}
