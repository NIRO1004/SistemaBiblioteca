using System.ComponentModel.DataAnnotations;
namespace SistemaBiblioteca.Models
{
    public class Autor
    {
        [Key]
        public int IdAutor { get; set; }

        [Required(ErrorMessage = "El nombre del autor es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        public string? Nacionalidad { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FechaNacimiento { get; set; }

        public ICollection<Libro> Libros { get; set; } = new List<Libro>();
    }
}
