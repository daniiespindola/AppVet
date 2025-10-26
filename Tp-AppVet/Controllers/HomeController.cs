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
            return View(); // Esto renderizar� Views/Home/Login.cshtml
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

            // Redirige al usuario a la p�gina de inicio 
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
            // 1. Finaliza el proceso de autenticaci�n iniciado por Challenge()
            // Esta l�nea lee la informaci�n del usuario de Google
            var authenticateResult = await HttpContext.AuthenticateAsync("Google");

            if (authenticateResult.Succeeded)
            {
                //    - Buscar el usuario en la tabla 'Usuarios' por el email proporcionado por Google.
                //    - Si no existe, crearlo.
                //    - Emitir el ticket de autenticaci�n local si es necesario (Sign In).

                // Asumiendo que el login fue exitoso y el usuario est� listo:

                // 3. Establecer el mensaje de �xito en TempData
                TempData["SuccessMessage"] = "�Inicio de sesi�n con Google exitoso! Bienvenido.";

                // 4. Redirigir a la p�gina principal para mostrar la alerta
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Manejar error de autenticaci�n (opcional)
                TempData["ErrorMessage"] = "Error al iniciar sesi�n con Google.";
                return RedirectToAction("Login");
            }
        }
    }
}
