using BibliotecaVirtual.Data;
using BibliotecaVirtual.Models;
using BibliotecaVirtual.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BibliotecaVirtual.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            var viewModel = new AdminDashboardViewModel();
            ViewData["UserRole"] = User.IsInRole("Admin") ? "Admin" : "Aprendiz"; // Para el layout

            if (User.IsInRole("Admin"))
            {
                // Si es Admin, carga todos los libros y todos los usuarios
                viewModel.Libros = _context.Libros.ToList();
                viewModel.Usuarios = _context.Usuarios.ToList();
            }
            else
            {
                // Si es Aprendiz, carga solo sus libros
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var usuarioId = int.Parse(userIdString);

                viewModel.Libros = _context.Libros.Where(l => l.UsuarioId == usuarioId).ToList();
                // Dejamos la lista de usuarios vacía a propósito
                viewModel.Usuarios = new List<Usuario>();

                ViewData["IsAdminDashboard"] = "true";
            }

            return View(viewModel);
        }

        // Este método solo muestra la página con el formulario vacío.
        public IActionResult AddBookPage()
        {
            // ViewData para el rol, si lo necesitas para el layout
            ViewData["UserRole"] = "Admin";
            return View();
        }

        [HttpPost]
        public IActionResult AddBook(Libro libro, IFormFile pdfFile)
        {
            if (ModelState.IsValid)
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                libro.UsuarioId = int.Parse(userIdString);

                if (pdfFile != null && pdfFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(pdfFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/pdfs/libros", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        pdfFile.CopyTo(stream);
                    }
                    libro.UrlArchivo = "/pdfs/libros/" + fileName;
                }

                _context.Libros.Add(libro);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }

            return View("AddBookPage", libro);
        }
        // --- MÉTODO GET PARA MOSTRAR EL FORMULARIO DE EDICIÓN ---
        public IActionResult Edit(int id)
        {
            // 1. Buscamos el libro en la base de datos por su Id.
            var libroAEditar = _context.Libros.FirstOrDefault(l => l.Id == id);

            // 2. Si no lo encontramos, devolvemos un error 404.
            if (libroAEditar == null)
            {
                return NotFound();
            }

            // Simulamos el rol para que el layout funcione
            ViewData["UserRole"] = "Admin";

            // 3. Enviamos el libro encontrado a la vista.
            return View(libroAEditar);
        }
        // --- MÉTODO POST PARA PROCESAR EL FORMULARIO DE EDICIÓN ---
        [HttpPost]
        public IActionResult Edit(Libro libroEditado)
        {
            if (ModelState.IsValid)
            {
                _context.Libros.Update(libroEditado);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }

            // Si el modelo NO es válido, volvemos a la vista de edición.
            ViewData["UserRole"] = "Admin";
            return View(libroEditado);
        }
        // --- MÉTODO GET PARA MOSTRAR LA PÁGINA DE CONFIRMACIÓN DE BORRADO ---
        public IActionResult Delete(int id)
        {
            var libroABorrar = _context.Libros.FirstOrDefault(l => l.Id == id);
            if (libroABorrar == null)
            {
                return NotFound();
            }
            ViewData["UserRole"] = "Admin";
            return View(libroABorrar);
        }
        // --- MÉTODO POST PARA EJECUTAR EL BORRADO ---
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            // 1. Buscamos el libro en la base de datos por su Id.
            var libroABorrar = _context.Libros.Find(id);

            // 2. Si lo encontramos, lo eliminamos.
            if (libroABorrar != null)
            {
                _context.Libros.Remove(libroABorrar);
                _context.SaveChanges(); // Guardamos los cambios en la BD
            }

            // 3. Redirigimos al usuario de vuelta al dashboard.
            return RedirectToAction("Dashboard");
        }
        // --- MÉTODO GET PARA MOSTRAR EL FORMULARIO DE EDICIÓN DE USUARIO ---
        public IActionResult EditUser(int id)
        {
            // Buscamos el usuario por su Id
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }

            ViewData["UserRole"] = "Admin"; // Para que el layout muestre el enlace de Admin
            return View(usuario);
        }
        // --- MÉTODO POST PARA GUARDAR LOS CAMBIOS DEL USUARIO ---
        [HttpPost]
        public IActionResult EditUser(Usuario usuarioEditado)
        {
            if (ModelState.IsValid)
            {
                _context.Usuarios.Update(usuarioEditado);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }

            // Si hay un error de validación, volvemos a mostrar el formulario
            ViewData["UserRole"] = "Admin";
            return View(usuarioEditado);
        }
        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            // Medida de seguridad: Evitar que un admin se elimine a sí mismo.
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id.ToString() == currentUserId)
            {
                return RedirectToAction("Dashboard");
            }

            var usuario = _context.Usuarios.Find(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                _context.SaveChanges();
            }

            return RedirectToAction("Dashboard");
        }
        // Error 500 simulación
        public IActionResult TriggerError()
        {
            throw new Exception("This is a simulated 500 Internal Server Error.");
        }
    }
}