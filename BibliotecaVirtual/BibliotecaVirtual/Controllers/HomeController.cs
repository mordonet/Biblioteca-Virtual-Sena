using Microsoft.AspNetCore.Mvc;

namespace BibliotecaVirtual.Controllers
{
    public class HomeController : Controller
    {
        // Esta acción se ejecuta cuando alguien visita la página de inicio
        public IActionResult Index()
        {

            ViewData["UserRole"] = "Admin";

            return View();
        }
    }
}
