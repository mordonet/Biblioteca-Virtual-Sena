using BibliotecaVirtual.Models;

namespace BibliotecaVirtual.ViewModels
{
    public class AdminDashboardViewModel
    {
        public List<Libro> Libros { get; set; }
        public List<Usuario> Usuarios { get; set; }
    }
}