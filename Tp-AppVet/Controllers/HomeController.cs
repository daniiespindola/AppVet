using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using Tp_AppVet.Data;
using Tp_AppVet.Models;

namespace Tp_AppVet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        public HomeController(ILogger<HomeController> logger,ApplicationDbContext context )
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

       
        public IActionResult Login()
        {
            return View(); // Esto renderizará Views/Home/Login.cshtml
        }

       
        [HttpPost] 
        public IActionResult IniciarSesionGoogle()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleLoginCallback")
            };

            // Esto redirige al usuario a Google.
            return Challenge(properties, "Google");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirige al usuario a la página de inicio 
            return RedirectToAction("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> GoogleLoginCallback()
        {
            // 1. Finaliza el proceso de autenticación iniciado por Challenge()
            // Esta línea lee la información del usuario de Google
            var authenticateResult = await HttpContext.AuthenticateAsync("Google");

            if (!authenticateResult.Succeeded)
            {
                // Manejar error de autenticación
                TempData["ErrorMessage"] = "Error al iniciar sesión con Google";
                return RedirectToAction("Login");
            }
            var email = authenticateResult.Principal.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                TempData["ErrorMessage"] = "No se pudo obtener el correo de Google.";
                return RedirectToAction("Login");
            }

            // Revisar si el usuario existe en la DB
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario == null)
            {
                // Asignar rol Admin si es tu correo, Cliente para otros
                string rol = (email == "rebecolque263@gmail.com") ? "Administrador" : "Cliente";
                usuario = new Usuario { Email = email, Rol = rol };
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();
            }

            // Crear claims incluyendo el rol
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, usuario.Rol)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Autenticación local
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);

            // Redirigir según rol
            return usuario.Rol switch
            {
                "Administrador" => RedirectToAction("Dashboard", "Admin"),
                "Veterinario" => RedirectToAction("Index", "VeterinarioDashboard"),
                "Cliente" => RedirectToAction("Index", "ClienteDashboard"),
                _ => RedirectToAction("Index", "Home")
            };
        }
    }
}
