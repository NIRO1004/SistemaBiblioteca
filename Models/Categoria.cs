using System.ComponentModel.DataAnnotations;
namespace SistemaBiblioteca.Models
{
    public class Categoria
    {
        [Key]
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(250)]
        public string? Descripcion { get; set; }

        public ICollection<Libro> Libros { get; set; } = new List<Libro>();
    }
}
