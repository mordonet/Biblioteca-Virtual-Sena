using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;// ¡Muy importante agregar este using!

namespace BibliotecaVirtual.Models
{
    public class Libro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(100, ErrorMessage = "El título no puede tener más de 100 caracteres.")]
        public string? Titulo { get; set; }

        [Required(ErrorMessage = "El autor es obligatorio.")]
        [StringLength(100)]
        public string? Autor { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        public string? Categoria { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "La URL de la portada es obligatoria.")]
        [Url(ErrorMessage = "Debe ser una URL válida.")]
        public string? UrlPortada { get; set; }
        public string? UrlArchivo { get; set; }
        public int? UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }
    }
}