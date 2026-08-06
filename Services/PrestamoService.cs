using Microsoft.EntityFrameworkCore;
using SistemaBiblioteca.Data;
using SistemaBiblioteca.Models;

namespace SistemaBiblioteca.Services
{
    public class PrestamoService
    {
        private readonly BibliotecaContext _context;

        public PrestamoService(BibliotecaContext context)
        {
            _context = context;
        }

        // Obtener todos los préstamos con usuario y libro
        public async Task<List<Prestamo>> ObtenerPrestamosAsync()
        {
            return await _context.Prestamos
                .Include(p => p.Usuario)
                .Include(p => p.Libro)
                .OrderByDescending(p => p.FechaPrestamo)
                .ToListAsync();
        }

        // Obtener un préstamo por Id
        public async Task<Prestamo?> ObtenerPorIdAsync(int id)
        {
            return await _context.Prestamos
                .Include(p => p.Usuario)
                .Include(p => p.Libro)
                .FirstOrDefaultAsync(p => p.IdPrestamo == id);
        }

        // Registrar un nuevo préstamo
        public async Task<(bool Exito, string Mensaje)> RegistrarPrestamoAsync(
            Prestamo prestamo)
        {
            var libro = await _context.Libros
                .FirstOrDefaultAsync(l => l.IdLibro == prestamo.IdLibro);

            if (libro == null)
            {
                return (false, "El libro seleccionado no existe.");
            }

            if (libro.CantidadDisponible <= 0)
            {
                return (false, "El libro no tiene ejemplares disponibles.");
            }

            prestamo.FechaPrestamo = DateTime.Now;
            prestamo.FechaDevolucion = null;
            prestamo.Estado = "Prestado";

            libro.CantidadDisponible--;

            _context.Prestamos.Add(prestamo);

            await _context.SaveChangesAsync();

            return (true, "El préstamo fue registrado correctamente.");
        }

        // Registrar la devolución de un préstamo
        public async Task<(bool Exito, string Mensaje)> RegistrarDevolucionAsync(
            int idPrestamo)
        {
            var prestamo = await _context.Prestamos
                .Include(p => p.Libro)
                .FirstOrDefaultAsync(p => p.IdPrestamo == idPrestamo);

            if (prestamo == null)
            {
                return (false, "El préstamo no existe.");
            }

            if (prestamo.Estado == "Devuelto")
            {
                return (false, "Este préstamo ya fue devuelto.");
            }

            prestamo.FechaDevolucion = DateTime.Now;
            prestamo.Estado = "Devuelto";

            prestamo.Libro.CantidadDisponible++;

            await _context.SaveChangesAsync();

            return (true, "La devolución fue registrada correctamente.");
        }

        // Consultar historial por usuario
        public async Task<List<Prestamo>> ObtenerHistorialPorUsuarioAsync(
            int idUsuario)
        {
            return await _context.Prestamos
                .Include(p => p.Usuario)
                .Include(p => p.Libro)
                .Where(p => p.IdUsuario == idUsuario)
                .OrderByDescending(p => p.FechaPrestamo)
                .ToListAsync();
        }
    }
}