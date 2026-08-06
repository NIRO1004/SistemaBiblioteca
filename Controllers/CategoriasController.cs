using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaBiblioteca.Models;
using SistemaBiblioteca.Services;

namespace SistemaBiblioteca.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly CategoriaService _categoriaService;

        public CategoriasController(CategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        // GET Categorias  ?busqueda=texto para filtrar por nombre
        public async Task<IActionResult> Index(string? busqueda)
        {
            var categorias = string.IsNullOrWhiteSpace(busqueda)
                ? await _categoriaService.ObtenerCategoriasAsync()
                : await _categoriaService.BuscarPorNombreAsync(busqueda);

            ViewData["Busqueda"] = busqueda;

            return View(categorias);
        }

        // GET Categorias/Details
        public async Task<IActionResult> Details(int? idcategoria)
        {
            if (idcategoria == null)
            {
                return NotFound();
            }

            var categoria = await _categoriaService.ObtenerCategoriaPorIdAsync(idcategoria.Value);

            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // GET Categorias/Libros  filtro: libros de una categoria especifica
        public async Task<IActionResult> Libros(int? idcategoria)
        {
            if (idcategoria == null)
            {
                return NotFound();
            }

            var categoria = await _categoriaService.ObtenerCategoriaPorIdAsync(idcategoria.Value);

            if (categoria == null)
            {
                return NotFound();
            }

            ViewData["Categoria"] = categoria;

            var libros = await _categoriaService.ObtenerLibrosPorCategoriaAsync(idcategoria.Value);

            return View(libros);
        }

        // GET Categorias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST Categorias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCategoria,Nombre,Descripcion")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                await _categoriaService.CrearCategoriaAsync(categoria);
                return RedirectToAction(nameof(Index));
            }

            return View(categoria);
        }

        // GET Categorias/Edit
        public async Task<IActionResult> Edit(int? idcategoria)
        {
            if (idcategoria == null)
            {
                return NotFound();
            }

            var categoria = await _categoriaService.ObtenerCategoriaPorIdAsync(idcategoria.Value);

            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // POST Categorias/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? idcategoria, [Bind("IdCategoria,Nombre,Descripcion")] Categoria categoria)
        {
            if (idcategoria != categoria.IdCategoria)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _categoriaService.ActualizarCategoriaAsync(categoria);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _categoriaService.ExisteCategoriaAsync(categoria.IdCategoria))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(categoria);
        }

        // GET Categorias/Delete
        public async Task<IActionResult> Delete(int? idcategoria)
        {
            if (idcategoria == null)
            {
                return NotFound();
            }

            var categoria = await _categoriaService.ObtenerCategoriaPorIdAsync(idcategoria.Value);

            if (categoria == null)
            {
                return NotFound();
            }

            // Se avisa en la vista si la categoria tiene libros asociados, para no romper la integridad
            ViewData["TieneLibros"] = await _categoriaService.TieneLibrosAsociadosAsync(idcategoria.Value);

            return View(categoria);
        }

        // POST Categorias/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int idcategoria)
        {
            // No se permite borrar una categoria que todavia tiene libros asociados
            if (await _categoriaService.TieneLibrosAsociadosAsync(idcategoria))
            {
                ModelState.AddModelError(string.Empty, "No se puede eliminar la categoría porque tiene libros asociados.");
                var categoria = await _categoriaService.ObtenerCategoriaPorIdAsync(idcategoria);
                ViewData["TieneLibros"] = true;
                return View(categoria);
            }

            await _categoriaService.EliminarCategoriaAsync(idcategoria);
            return RedirectToAction(nameof(Index));
        }
    }
}
