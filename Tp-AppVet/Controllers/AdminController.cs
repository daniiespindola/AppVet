using Microsoft.AspNetCore.Mvc;

namespace Tp_AppVet.Controllers
{
    public class AdminController : Controller
    {
        //GET: /Admin/Dashboard
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
