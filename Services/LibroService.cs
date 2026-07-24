using Microsoft.EntityFrameworkCore;
using SistemaBiblioteca.Data;
using SistemaBiblioteca.Models;

namespace SistemaBiblioteca.Services
{
    public class LibroService
    {
        private readonly BibliotecaContext _context;

        public LibroService(BibliotecaContext context)
        {
            _context = context;
        }

        //De aca se optinen todos los libros
        public async Task<List<Libro>> ObtenerLibrosAsync()
        {
            return await _context.Libros
                .Include(l => l.Autor)
                .Include(l => l.Categoria)
                .ToListAsync();
        }

        //De aca se buscan los libros por titulo
        public async Task<List<Libro>> BuscarPorTituloAsync(string titulo)
        {
            return await _context.Libros
                .Include(l => l.Autor)
                .Include(l => l.Categoria)
                .Where(l => l.Titulo.Contains(titulo))
                .ToListAsync();
        }

        //De aca buscan los libros por autor
        public async Task<List<Libro>> BuscarPorAutorAsync(int idAutor)
        {
            return await _context.Libros
                .Include(l => l.Autor)
                .Include(l => l.Categoria)
                .Where(l => l.IdAutor == idAutor)
                .ToListAsync();
        }

        //De aca buscan los libros por categoria
        public async Task<List<Libro>> BuscarPorCatergoriaAsync(int idCategoria)
        {
            return await _context.Libros
                            .Include(l => l.Autor)
                            .Include(l => l.Categoria)
                            .Where(l => l.IdCategoria == idCategoria)
                            .ToListAsync();
        }
    }
}