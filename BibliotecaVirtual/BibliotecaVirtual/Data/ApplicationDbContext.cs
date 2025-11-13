using BibliotecaVirtual.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaVirtual.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Libro> Libros { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Favorito> Favoritos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1, // El Id debe ser explícito
                    Nombre = "Administrador Principal",
                    Correo = "admin@biblioteca.com",
                    Password = "admin123", // Cambia esto por una contraseña segura
                    Rol = "Admin"
                }
            );
        }
    }
}