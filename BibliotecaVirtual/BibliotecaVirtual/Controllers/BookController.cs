using BibliotecaVirtual.Data; // Necesitamos acceso a nuestro DbContext
using BibliotecaVirtual.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace BibliotecaVirtual.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _context;

        // El constructor recibe una instancia del DbContext
        public BookController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Acción para mostrar el catálogo completo
        public IActionResult Catalog(string searchString, string bookCategory)
        {
            ViewData["UserRole"] = "Admin";

            var categories = _context.Libros
                                     .OrderBy(l => l.Categoria)
                                     .Select(l => l.Categoria)
                                     .Distinct()
                                     .ToList();

            ViewBag.BookCategory = new SelectList(categories);

            // --- INICIO DE LA NUEVA LÓGICA ---
            // Verificamos si el usuario ha iniciado sesión para buscar sus favoritos
            if (User.Identity.IsAuthenticated)
            {
                // 1. Obtenemos el Id del usuario.
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var usuarioId = int.Parse(userIdString);

                // 2. Obtenemos una lista SOLO con los Ids de sus libros favoritos.
                var librosFavoritosIds = _context.Favoritos
                                                 .Where(f => f.UsuarioId == usuarioId)
                                                 .Select(f => f.LibroId)
                                                 .ToList();

                // 3. Pasamos esta lista a la vista usando ViewBag.
                ViewBag.LibrosFavoritos = librosFavoritosIds;
            }
            // --- FIN DE LA NUEVA LÓGICA ---

            var librosQuery = _context.Libros.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                librosQuery = librosQuery.Where(l => l.Titulo.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(bookCategory))
            {
                librosQuery = librosQuery.Where(l => l.Categoria == bookCategory);
            }

            var libros = librosQuery.Select(l => new Libro
            {
                Id = l.Id,
                Titulo = l.Titulo ?? "",
                Autor = l.Autor ?? "",
                Categoria = l.Categoria ?? "",
                Descripcion = l.Descripcion ?? "",
                UrlPortada = l.UrlPortada ?? "",
                UrlArchivo = l.UrlArchivo ?? ""
            }).ToList();

            return View(libros);
        }

        // Acción para mostrar los detalles de UN solo libro
        public IActionResult Details(int id)
        {
            // Buscamos el libro por su Id EN LA BASE DE DATOS
            Libro libro = _context.Libros.FirstOrDefault(l => l.Id == id);

            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }
    }
}
