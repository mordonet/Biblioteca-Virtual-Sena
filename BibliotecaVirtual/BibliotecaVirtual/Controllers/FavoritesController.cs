using BibliotecaVirtual.Data;
using BibliotecaVirtual.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaVirtual.Controllers
{
    [Authorize] // Solo usuarios autenticados pueden acceder a esto
    public class FavoritesController : Controller
    {
        public IActionResult Index()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var usuarioId = int.Parse(userIdString);
            var favoritos = _context.Favoritos
                                    .Where(f  => f.UsuarioId == usuarioId)
                                    .Include(f => f.Libro)
                                    .ToList();
            return View(favoritos);
        }
        private readonly ApplicationDbContext _context;

        public FavoritesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Add(int libroId)
        {
            // 1. Obtenemos el Id del usuario que está actualmente logueado
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var usuarioId = int.Parse(userIdString);

            // 2. Verificamos si ya existe este favorito
            var yaExiste = _context.Favoritos
                .Any(f => f.UsuarioId == usuarioId && f.LibroId == libroId);

            if (!yaExiste)
            {
                // 3. Si no existe, creamos el nuevo favorito y lo guardamos
                var favorito = new Favorito
                {
                    UsuarioId = usuarioId,
                    LibroId = libroId
                };
                _context.Favoritos.Add(favorito);
                _context.SaveChanges();
            }

            // 4. Redirigimos de vuelta a la página de detalles del libro
            return RedirectToAction("Details", "Book", new { id = libroId });
        }
        // --- MÉTODO POST PARA QUITAR UN FAVORITO ---
        [HttpPost]
        public IActionResult Remove(int libroId)
        {
            // 1. Obtenemos el Id del usuario logueado
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var usuarioId = int.Parse(userIdString);

            // 2. Buscamos el registro específico en la tabla Favoritos
            var favorito = _context.Favoritos
                                   .FirstOrDefault(f => f.UsuarioId == usuarioId && f.LibroId == libroId);

            if (favorito != null)
            {
                // 3. Si lo encontramos, lo eliminamos y guardamos los cambios
                _context.Favoritos.Remove(favorito);
                _context.SaveChanges();
            }

            // 4. Redirigimos de vuelta a la página de favoritos del usuario
            return RedirectToAction("Index");
        }
    }
}