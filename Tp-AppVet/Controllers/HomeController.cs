using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Tp_AppVet.Models;

namespace Tp_AppVet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
            var properties = new Microsoft.AspNetCore.Authentication.AuthenticationProperties
            {
                RedirectUri = Url.Action("Create", "Usuarios")
            };

            // Esto redirige al usuario a Google.
            return Challenge(properties, "Google");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirige al usuario a la página de inicio 
            return RedirectToAction("Index", "Home");
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

            if (authenticateResult.Succeeded)
            {
                //    - Buscar el usuario en la tabla 'Usuarios' por el email proporcionado por Google.
                //    - Si no existe, crearlo.
                //    - Emitir el ticket de autenticación local si es necesario (Sign In).

                // Asumiendo que el login fue exitoso y el usuario está listo:

                // 3. Establecer el mensaje de éxito en TempData
                TempData["SuccessMessage"] = "¡Inicio de sesión con Google exitoso! Bienvenido.";

                // 4. Redirigir a la página principal para mostrar la alerta
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Manejar error de autenticación (opcional)
                TempData["ErrorMessage"] = "Error al iniciar sesión con Google.";
                return RedirectToAction("Login");
            }
        }
    }
}
