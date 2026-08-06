using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SistemaBiblioteca.Models
{
    public class Libro
    {
        [Key]
        public int IdLibro { get; set; }

        [Required(ErrorMessage = "El título del libro es obligatorio.")]
        [StringLength(150)]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El ISBN es obligatorio.")]
        [StringLength(20)]
        public string ISBN { get; set; } = string.Empty;

        public int? AnioPublicacion { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "La cantidad total no puede ser negativa.")]
        public int CantidadTotal { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "La cantidad disponible no puede ser negativa.")]
        public int CantidadDisponible { get; set; }

        [Required]
        public int IdAutor { get; set; }

        [ForeignKey(nameof(IdAutor))]
        [ValidateNever]
        public Autor Autor { get; set; } = null!;

        [Required]
        public int IdCategoria { get; set; }

        [ForeignKey(nameof(IdCategoria))]
        [ValidateNever]
        public Categoria Categoria { get; set; } = null!;

        public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    }
}