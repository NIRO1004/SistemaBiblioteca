using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaBiblioteca.Models
{
    public class Prestamo
    {
        [Key]
        public int IdPrestamo { get; set; }

        [Required]
        public int IdUsuario { get; set; }

        [ForeignKey(nameof(IdUsuario))]
        public Usuario Usuario { get; set; } = null!;

        [Required]
        public int IdLibro { get; set; }

        [ForeignKey(nameof(IdLibro))]
        public Libro Libro { get; set; } = null!;

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaPrestamo { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FechaDevolucion { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Prestado";
    }
}