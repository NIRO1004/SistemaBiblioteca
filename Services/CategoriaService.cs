using Microsoft.EntityFrameworkCore;
using SistemaBiblioteca.Data;
using SistemaBiblioteca.Models;

namespace SistemaBiblioteca.Services
{
    public class CategoriaService
    {
        private readonly BibliotecaContext _context;

        public CategoriaService(BibliotecaContext context)
        {
            _context = context;
        }

        // De aca se obtienen todas las categorias
        public async Task<List<Categoria>> ObtenerCategoriasAsync()
        {
            return await _context.Categorias
                .Include(c => c.Libros)
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }

        // De aqi se busca una categoria por su Id, incluyendo sus libros
        public async Task<Categoria?> ObtenerCategoriaPorIdAsync(int idCategoria)
        {
            return await _context.Categorias
                .Include(c => c.Libros)
                    .ThenInclude(l => l.Autor)
                .FirstOrDefaultAsync(c => c.IdCategoria == idCategoria);
        }

        // este otro busca las categorias por nombre (filtro de búsqueda)
        public async Task<List<Categoria>> BuscarPorNombreAsync(string nombre)
        {
            return await _context.Categorias
                .Include(c => c.Libros)
                .Where(c => c.Nombre.Contains(nombre))
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }

        // De aca se obtienen los libros de una categoria especifica (filtro por categoria)
        public async Task<List<Libro>> ObtenerLibrosPorCategoriaAsync(int idCategoria)
        {
            return await _context.Libros
                .Include(l => l.Autor)
                .Where(l => l.IdCategoria == idCategoria)
                .ToListAsync();
        }

        public async Task<bool> ExisteCategoriaAsync(int idCategoria)
        {
            return await _context.Categorias.AnyAsync(c => c.IdCategoria == idCategoria);
        }

        // Verifica si la categoria tiene libros asociados, para evitar borrarla si los tiene
        //OJO no tocar este codigoooo
        public async Task<bool> TieneLibrosAsociadosAsync(int idCategoria)
        {
            return await _context.Libros.AnyAsync(l => l.IdCategoria == idCategoria);
        }

        public async Task CrearCategoriaAsync(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarCategoriaAsync(Categoria categoria)
        {
            _context.Categorias.Update(categoria);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarCategoriaAsync(int idCategoria)
        {
            var categoria = await _context.Categorias.FindAsync(idCategoria);

            if (categoria != null)
            {
                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();
            }
        }
    }
}
