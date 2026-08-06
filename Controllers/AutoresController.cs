using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaBiblioteca.Models;
using SistemaBiblioteca.Services;

namespace SistemaBiblioteca.Controllers
{
    public class AutoresController : Controller
    {
        private readonly AutorService _autorService;

        public AutoresController(AutorService autorService)
        {
            _autorService = autorService;
        }

        // GET Autores  ?busqueda=texto para filtrar por nombre
        public async Task<IActionResult> Index(string? busqueda)
        {
            var autores = string.IsNullOrWhiteSpace(busqueda)
                ? await _autorService.ObtenerAutoresAsync()
                : await _autorService.BuscarPorNombreAsync(busqueda);

            ViewData["Busqueda"] = busqueda;

            return View(autores);
        }

        // GET Autores/Details
        public async Task<IActionResult> Details(int? idautor)
        {
            if (idautor == null)
            {
                return NotFound();
            }

            var autor = await _autorService.ObtenerAutorPorIdAsync(idautor.Value);

            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        // GET Autores/Libros/5  (iltro: libros de un autor especifico
        public async Task<IActionResult> Libros(int? idautor)
        {
            if (idautor == null)
            {
                return NotFound();
            }

            var autor = await _autorService.ObtenerAutorPorIdAsync(idautor.Value);

            if (autor == null)
            {
                return NotFound();
            }

            ViewData["Autor"] = autor;

            var libros = await _autorService.ObtenerLibrosPorAutorAsync(idautor.Value);

            return View(libros);
        }

        // GET Autores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST Autores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAutor,Nombre,Nacionalidad,FechaNacimiento")] Autor autor)
        {
            if (ModelState.IsValid)
            {
                await _autorService.CrearAutorAsync(autor);
                return RedirectToAction(nameof(Index));
            }

            return View(autor);
        }

        // GET Autores/Edit
        public async Task<IActionResult> Edit(int? idautor)
        {
            if (idautor == null)
            {
                return NotFound();
            }

            var autor = await _autorService.ObtenerAutorPorIdAsync(idautor.Value);

            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        // POST Autores/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? idautor, [Bind("IdAutor,Nombre,Nacionalidad,FechaNacimiento")] Autor autor)
        {
            if (idautor != autor.IdAutor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _autorService.ActualizarAutorAsync(autor);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _autorService.ExisteAutorAsync(autor.IdAutor))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(autor);
        }

        // GET Autores/Delete
        public async Task<IActionResult> Delete(int? idautor)
        {
            if (idautor == null)
            {
                return NotFound();
            }

            var autor = await _autorService.ObtenerAutorPorIdAsync(idautor.Value);

            if (autor == null)
            {
                return NotFound();
            }

            // Se avisa en la vista si el autor tiene libros asociados, para no romper la integridad
            ViewData["TieneLibros"] = await _autorService.TieneLibrosAsociadosAsync(idautor.Value);

            return View(autor);
        }

        // POST Autores/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int idautor)
        {
            // No deja borrar un autor que todavia tiene libros asociados
            if (await _autorService.TieneLibrosAsociadosAsync(idautor))
            {
                ModelState.AddModelError(string.Empty, "No se puede eliminar el autor porque tiene libros asociados.");
                var autor = await _autorService.ObtenerAutorPorIdAsync(idautor);
                ViewData["TieneLibros"] = true;
                return View(autor);
            }

            await _autorService.EliminarAutorAsync(idautor);
            return RedirectToAction(nameof(Index));
        }
    }
}
