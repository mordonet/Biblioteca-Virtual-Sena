using Microsoft.AspNetCore.Mvc;

namespace BibliotecaVirtual.Controllers
{
    public class ErrorController : Controller
    {
        // Esta acción se activará para manejar diferentes errores de estado HTTP
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Lo sentimos, la página que buscas no existe.";
                    return View("NotFound404");
                case 500:
                    ViewBag.ErrorMessage = "Lo sentimos, algo salió mal en el servidor. Inténtalo de nuevo más tarde.";
                    return View("Error500");
            }

            // Para cualquier otro error, mostramos una vista genérica
            ViewBag.ErrorMessage = $"Lo sentimos, ocurrió un error inesperado (Código: {statusCode}).";
            return View("Error500");
        }
    }
}