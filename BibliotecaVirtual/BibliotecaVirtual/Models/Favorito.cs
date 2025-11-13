using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaVirtual.Models
{
    public class Favorito
    {
        [Key] // Indica que esta es la clave primaria
        public int Id { get; set; }

        // Clave foránea para el Usuario
        public int UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }

        // Clave foránea para el Libro
        public int LibroId { get; set; }
        [ForeignKey("LibroId")]
        public Libro Libro { get; set; }
    }
}