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
    public class TurnoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TurnoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Turnoes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Turnos.Include(t => t.Cliente).Include(t => t.Mascota).Include(t => t.Veterinario);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Turnoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turno = await _context.Turnos
                .Include(t => t.Cliente)
                .Include(t => t.Mascota)
                .Include(t => t.Veterinario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (turno == null)
            {
                return NotFound();
            }

            return View(turno);
        }

        // GET: Turnoes/Create
        public async Task<IActionResult> Create()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
            {
                TempData["ErrorMessage"] = "Usuario no encontrado.";
                return RedirectToAction("Index", "Home");
            }

            // Solo veterinarios o administradores pueden crear turnos
            if (usuario.Rol != "Veterinario" && usuario.Rol != "Administrador")
            {
                TempData["ErrorMessage"] = "No tenés permiso para crear turnos.";
                return RedirectToAction("Index", "Home");
            }

            // Validar que haya al menos un cliente
            var hayClientes = await _context.Clientes.AnyAsync();
            if (!hayClientes)
            {
                TempData["ErrorMessage"] = "Debés registrar un cliente antes de crear un turno.";
                return RedirectToAction("Index", "VeterinarioDashboard");
            }

            // Validar que haya al menos una mascota
            var hayMascotas = await _context.Mascotas.AnyAsync();
            if (!hayMascotas)
            {
                TempData["ErrorMessage"] = "Necesitás registrar una mascota antes de crear un turno.";
                return RedirectToAction("Index", "VeterinarioDashboard");
            }

            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido");
            ViewData["MascotaId"] = new SelectList(_context.Mascotas, "Id", "Especie");
            ViewData["VeterinarioId"] = new SelectList(_context.Veterinarios, "Id", "Apellido");
            return View();
        }

        // POST: Turnoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fecha,Motivo,ClienteId,MascotaId,VeterinarioId")] Turno turno)
        {
            if (ModelState.IsValid)
            {
                _context.Add(turno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", turno.ClienteId);
            ViewData["MascotaId"] = new SelectList(_context.Mascotas, "Id", "Especie", turno.MascotaId);
            ViewData["VeterinarioId"] = new SelectList(_context.Veterinarios, "Id", "Apellido", turno.VeterinarioId);
            return View(turno);
        }

        // GET: Turnoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", turno.ClienteId);
            ViewData["MascotaId"] = new SelectList(_context.Mascotas, "Id", "Especie", turno.MascotaId);
            ViewData["VeterinarioId"] = new SelectList(_context.Veterinarios, "Id", "Apellido", turno.VeterinarioId);
            return View(turno);
        }

        // POST: Turnoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fecha,Motivo,ClienteId,MascotaId,VeterinarioId")] Turno turno)
        {
            if (id != turno.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(turno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TurnoExists(turno.Id))
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
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", turno.ClienteId);
            ViewData["MascotaId"] = new SelectList(_context.Mascotas, "Id", "Especie", turno.MascotaId);
            ViewData["VeterinarioId"] = new SelectList(_context.Veterinarios, "Id", "Apellido", turno.VeterinarioId);
            return View(turno);
        }

        // GET: Turnoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turno = await _context.Turnos
                .Include(t => t.Cliente)
                .Include(t => t.Mascota)
                .Include(t => t.Veterinario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (turno == null)
            {
                return NotFound();
            }

            return View(turno);
        }

        // POST: Turnoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var turno = await _context.Turnos.FindAsync(id);
            if (turno != null)
            {
                _context.Turnos.Remove(turno);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TurnoExists(int id)
        {
            return _context.Turnos.Any(e => e.Id == id);
        }
    }
}
