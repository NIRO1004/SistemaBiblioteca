using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaBiblioteca.Data;
using SistemaBiblioteca.Models;
using SistemaBiblioteca.Services;

namespace SistemaBiblioteca.Controllers
{
    [Authorize]
    public class PrestamosController : Controller
    {
        private readonly PrestamoService _prestamoService;
        private readonly BibliotecaContext _context;

        public PrestamosController(
            PrestamoService prestamoService,
            BibliotecaContext context)
        {
            _prestamoService = prestamoService;
            _context = context;
        }

        // GET: Prestamos
        // Muestra todos los préstamos registrados.
        public async Task<IActionResult> Index()
        {
            var prestamos = await _prestamoService.ObtenerPrestamosAsync();

            return View(prestamos);
        }

        // GET: Prestamos/Details/5
        // Muestra la información de un préstamo específico.
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamo = await _prestamoService.ObtenerPorIdAsync(id.Value);

            if (prestamo == null)
            {
                return NotFound();
            }

            return View(prestamo);
        }

        // GET: Prestamos/Create
        // Abre el formulario para registrar un préstamo.
        public IActionResult Create()
        {
            CargarListas();

            return View();
        }

        // POST: Prestamos/Create
        // Registra un préstamo nuevo.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("IdUsuario,IdLibro")] Prestamo prestamo)
        {
            /*
             * Estas propiedades no vienen desde el formulario.
             * El servicio les asigna sus valores o Entity Framework
             * las relaciona mediante IdUsuario e IdLibro.
             */
            ModelState.Remove(nameof(Prestamo.Usuario));
            ModelState.Remove(nameof(Prestamo.Libro));
            ModelState.Remove(nameof(Prestamo.FechaPrestamo));
            ModelState.Remove(nameof(Prestamo.FechaDevolucion));
            ModelState.Remove(nameof(Prestamo.Estado));

            if (prestamo.IdUsuario <= 0)
            {
                ModelState.AddModelError(
                    nameof(Prestamo.IdUsuario),
                    "Debe seleccionar un usuario.");
            }

            if (prestamo.IdLibro <= 0)
            {
                ModelState.AddModelError(
                    nameof(Prestamo.IdLibro),
                    "Debe seleccionar un libro.");
            }

            if (!ModelState.IsValid)
            {
                CargarListas(
                    prestamo.IdUsuario,
                    prestamo.IdLibro);

                return View(prestamo);
            }

            var resultado =
                await _prestamoService.RegistrarPrestamoAsync(prestamo);

            if (!resultado.Exito)
            {
                ModelState.AddModelError(
                    string.Empty,
                    resultado.Mensaje);

                CargarListas(
                    prestamo.IdUsuario,
                    prestamo.IdLibro);

                return View(prestamo);
            }

            TempData["Mensaje"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }

        // POST: Prestamos/Devolver/5
        // Registra la devolución de un libro.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Devolver(int id)
        {
            var resultado =
                await _prestamoService.RegistrarDevolucionAsync(id);

            if (resultado.Exito)
            {
                TempData["Mensaje"] = resultado.Mensaje;
            }
            else
            {
                TempData["Error"] = resultado.Mensaje;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Prestamos/Historial/5
        // Muestra el historial de préstamos de un usuario.
        public async Task<IActionResult> Historial(int? idUsuario)
        {
            if (idUsuario == null)
            {
                return NotFound();
            }

            var prestamos =
                await _prestamoService.ObtenerHistorialPorUsuarioAsync(
                    idUsuario.Value);

            ViewData["IdUsuario"] = idUsuario.Value;

            return View(prestamos);
        }

        // Carga las listas de usuarios y libros para el formulario.
        private void CargarListas(
            int? idUsuarioSeleccionado = null,
            int? idLibroSeleccionado = null)
        {
            ViewData["IdUsuario"] = new SelectList(
                _context.Usuarios,
                "IdUsuario",
                "Nombre",
                idUsuarioSeleccionado);

            ViewData["IdLibro"] = new SelectList(
                _context.Libros
                    .Where(l => l.CantidadDisponible > 0),
                "IdLibro",
                "Titulo",
                idLibroSeleccionado);
        }
    }
}