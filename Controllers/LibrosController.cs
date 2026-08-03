using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaBiblioteca.Data;
using SistemaBiblioteca.Models;

namespace SistemaBiblioteca.Controllers
{
    public class LibrosController : Controller
    {
        private readonly BibliotecaContext _context;

        public LibrosController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: Libros
        public async Task<IActionResult> Index()
        {
            var libros = _context.Libros
                .Include(l => l.Autor)
                .Include(l => l.Categoria);

            return View(await libros.ToListAsync());
        }

        // GET: Libros/Details/5
        public async Task<IActionResult> Details(int? idlibro)
        {
            if (idlibro == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .Include(l => l.Autor)
                .Include(l => l.Categoria)
                .FirstOrDefaultAsync(l => l.IdLibro == idlibro);

            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // GET: Libros/Create
        public IActionResult Create()
        {
            ViewData["IdAutor"] = new SelectList(_context.Autores, "IdAutor", "Nombre");
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "Nombre");

            return View();
        }

        // POST: Libros/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdLibro,Titulo,ISBN,AnioPublicacion,CantidadTotal,CantidadDisponible,IdAutor,IdCategoria")] Libro libro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(libro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdAutor"] = new SelectList(_context.Autores, "IdAutor", "Nombre", libro.IdAutor);
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "Nombre", libro.IdCategoria);

            return View(libro);
        }

        // GET: Libros/Edit/5
        public async Task<IActionResult> Edit(int? idlibro)
        {
            if (idlibro == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros.FindAsync(idlibro);

            if (libro == null)
            {
                return NotFound();
            }

            ViewData["IdAutor"] = new SelectList(_context.Autores, "IdAutor", "Nombre", libro.IdAutor);
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "Nombre", libro.IdCategoria);

            return View(libro);
        }

        // POST: Libros/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? idlibro, [Bind("IdLibro,Titulo,ISBN,AnioPublicacion,CantidadTotal,CantidadDisponible,IdAutor,IdCategoria")] Libro libro)
        {
            if (idlibro != libro.IdLibro)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(libro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibroExists(libro.IdLibro))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["IdAutor"] = new SelectList(_context.Autores, "IdAutor", "Nombre", libro.IdAutor);
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "Nombre", libro.IdCategoria);

            return View(libro);
        }

        // GET: Libros/Delete/5
        public async Task<IActionResult> Delete(int? idlibro)
        {
            if (idlibro == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .Include(l => l.Autor)
                .Include(l => l.Categoria)
                .FirstOrDefaultAsync(l => l.IdLibro == idlibro);

            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // POST: Libros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? idlibro)
        {
            var libro = await _context.Libros.FindAsync(idlibro);

            if (libro != null)
            {
                _context.Libros.Remove(libro);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool LibroExists(int? idlibro)
        {
            return _context.Libros.Any(e => e.IdLibro == idlibro);
        }
    }
}