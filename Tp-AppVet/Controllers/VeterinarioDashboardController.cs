using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tp_AppVet.Controllers
{
    [Authorize(Roles = "Administrador,Veterinario")]
    public class VeterinarioDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
