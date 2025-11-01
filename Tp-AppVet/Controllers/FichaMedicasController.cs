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
    [Authorize(Roles = "Veterinario,Administrador")]
    public class FichaMedicasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FichaMedicasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FichaMedicas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.FichaMedicas.Include(f => f.Mascota);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: FichaMedicas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fichaMedica = await _context.FichaMedicas
                .Include(f => f.Mascota)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fichaMedica == null)
            {
                return NotFound();
            }

            return View(fichaMedica);
        }

        // GET: FichaMedicas/Create
        public async Task<IActionResult> Create(int? mascotaId)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
            {
                TempData["ErrorMessage"] = "Usuario no encontrado.";
                return RedirectToAction("Index", "Home");
            }

            // Solo veterinarios o administradores pueden crear fichas
            if (usuario.Rol != "Veterinario" && usuario.Rol != "Administrador")
            {
                TempData["ErrorMessage"] = "No tenés permiso para crear fichas médicas.";
                return RedirectToAction("Index", "Home");
            }

            var hayMascotas = await _context.Mascotas.AnyAsync();
            // Verificar que se haya pasado una mascota
            if (!hayMascotas)
            {
                TempData["ErrorMessage"] = "Debés registrar una mascota antes de crear una Ficha Médica.";
                return RedirectToAction("Index", "VeterinarioDashboard");
            }

            // Verificar que la mascota exista
            if (mascotaId.HasValue)
            {
                var mascota = await _context.Mascotas.FindAsync(mascotaId);
                if (mascota == null)
                {
                    TempData["ErrorMessage"] = "La mascota seleccionada no existe.";
                    return RedirectToAction("Index", "VeterinarioDashboard");
                }

            }
            ViewData["MascotaId"] = new SelectList(_context.Mascotas, "Id", "Especie");
            return View();
        }

        // POST: FichaMedicas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FechaCreacion,Observaciones,MascotaId")] FichaMedica fichaMedica)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fichaMedica);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MascotaId"] = new SelectList(_context.Mascotas, "Id", "Especie", fichaMedica.MascotaId);
            return View(fichaMedica);
        }

        // GET: FichaMedicas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fichaMedica = await _context.FichaMedicas.FindAsync(id);
            if (fichaMedica == null)
            {
                return NotFound();
            }
            ViewData["MascotaId"] = new SelectList(_context.Mascotas, "Id", "Especie", fichaMedica.MascotaId);
            return View(fichaMedica);
        }

        // POST: FichaMedicas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaCreacion,Observaciones,MascotaId")] FichaMedica fichaMedica)
        {
            if (id != fichaMedica.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fichaMedica);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FichaMedicaExists(fichaMedica.Id))
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
            ViewData["MascotaId"] = new SelectList(_context.Mascotas, "Id", "Especie", fichaMedica.MascotaId);
            return View(fichaMedica);
        }

        // GET: FichaMedicas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fichaMedica = await _context.FichaMedicas
                .Include(f => f.Mascota)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fichaMedica == null)
            {
                return NotFound();
            }

            return View(fichaMedica);
        }

        // POST: FichaMedicas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fichaMedica = await _context.FichaMedicas.FindAsync(id);
            if (fichaMedica != null)
            {
                _context.FichaMedicas.Remove(fichaMedica);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FichaMedicaExists(int id)
        {
            return _context.FichaMedicas.Any(e => e.Id == id);
        }
    }
}
