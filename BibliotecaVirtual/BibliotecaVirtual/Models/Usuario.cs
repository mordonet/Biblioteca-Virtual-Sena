using System.ComponentModel.DataAnnotations;

namespace BibliotecaVirtual.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Password { get; set; } // Más adelante guardaremos esto de forma segura (hash)
        public string? Rol { get; set; } // Será "Admin" o "Aprendiz"
    }
}