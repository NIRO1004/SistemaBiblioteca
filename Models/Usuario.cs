using System.ComponentModel.DataAnnotations;
namespace SistemaBiblioteca.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre del usuario es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo válido.")]
        [StringLength(150)]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength (100)]
        public string Contrasena {  get; set; } = string.Empty;

        [Required(ErrorMessage = "El rol es obligatorio")]
        [StringLength (30)]
        public string Rol {  get; set; } = string.Empty;

        public ICollection<Prestamo> Prestamo { get; set; } = new List<Prestamo>();
    }
}
