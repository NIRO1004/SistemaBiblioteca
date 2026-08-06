using Microsoft.EntityFrameworkCore;
using SistemaBiblioteca.Data;
using SistemaBiblioteca.Models;

namespace SistemaBiblioteca.Services
{
    public class AutorService
    {
        private readonly BibliotecaContext _context;

        public AutorService(BibliotecaContext context)
        {
            _context = context;
        }

        // De este lado se obtienen todos los autores
        public async Task<List<Autor>> ObtenerAutoresAsync()
        {
            return await _context.Autores
                .Include(a => a.Libros)
                .OrderBy(a => a.Nombre)
                .ToListAsync();
        }

        // De este otro se busca un autor por su Id, incluyendo sus libros
        public async Task<Autor?> ObtenerAutorPorIdAsync(int idAutor)
        {
            return await _context.Autores
                .Include(a => a.Libros)
                    .ThenInclude(l => l.Categoria)
                .FirstOrDefaultAsync(a => a.IdAutor == idAutor);
        }

        // De aca se buscan los autores por nombre (los que seria el filtro de búsqueda)
        public async Task<List<Autor>> BuscarPorNombreAsync(string nombre)
        {
            return await _context.Autores
                .Include(a => a.Libros)
                .Where(a => a.Nombre.Contains(nombre))
                .OrderBy(a => a.Nombre)
                .ToListAsync();
        }

        // aca se obtienen los libros de un autor especifico filtro por autor
        public async Task<List<Libro>> ObtenerLibrosPorAutorAsync(int idAutor)
        {
            return await _context.Libros
                .Include(l => l.Categoria)
                .Where(l => l.IdAutor == idAutor)
                .ToListAsync();
        }

        public async Task<bool> ExisteAutorAsync(int idAutor)
        {
            return await _context.Autores.AnyAsync(a => a.IdAutor == idAutor);
        }

        // Verifica si el autor tiene libros asociados para evitar borrarlo si los tiene
        // Ojo no tocar este codigo
        public async Task<bool> TieneLibrosAsociadosAsync(int idAutor)
        {
            return await _context.Libros.AnyAsync(l => l.IdAutor == idAutor);
        }

        public async Task CrearAutorAsync(Autor autor)
        {
            _context.Autores.Add(autor);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAutorAsync(Autor autor)
        {
            _context.Autores.Update(autor);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAutorAsync(int idAutor)
        {
            var autor = await _context.Autores.FindAsync(idAutor);

            if (autor != null)
            {
                _context.Autores.Remove(autor);
                await _context.SaveChangesAsync();
            }
        }
    }
}
