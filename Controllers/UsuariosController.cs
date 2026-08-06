using Microsoft.AspNetCore.Mvc;
using SistemaBiblioteca.Models;
using SistemaBiblioteca.Services;
using Microsoft.AspNetCore.Authorization;

namespace SistemaBiblioteca.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly UsuarioService _usuarioService;

        public UsuariosController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // GET: Usuarios
        // Muestra todos los usuarios registrados.
        public async Task<IActionResult> Index(string? nombre)
        {
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                var usuariosEncontrados =
                    await _usuarioService.BuscarPorNombreAsync(nombre);

                ViewData["NombreBuscado"] = nombre;

                return View(usuariosEncontrados);
            }

            var usuarios = await _usuarioService.ObtenerUsuariosAsync();

            return View(usuarios);
        }

        // GET: Usuarios/Details/5
        // Muestra la información de un usuario específico.
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _usuarioService.ObtenerPorIdAsync(id.Value);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        // Abre el formulario para crear un usuario.
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // Recibe y guarda el nuevo usuario.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Nombre,Correo,Contrasena,Rol")] Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return View(usuario);
            }

            var usuarioExistente =
                await _usuarioService.BuscarPorCorreoAsync(usuario.Correo);

            if (usuarioExistente != null)
            {
                ModelState.AddModelError(
                    "Correo",
                    "Ya existe un usuario registrado con este correo.");

                return View(usuario);
            }

            await _usuarioService.AgregarUsuarioAsync(usuario);

            TempData["Mensaje"] = "El usuario fue creado correctamente.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Usuarios/Edit/5
        // Abre el formulario para modificar un usuario.
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _usuarioService.ObtenerPorIdAsync(id.Value);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // Guarda los cambios realizados al usuario.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("IdUsuario,Nombre,Correo,Contrasena,Rol")] Usuario usuario)
        {
            if (id != usuario.IdUsuario)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(usuario);
            }

            var usuarioConMismoCorreo =
                await _usuarioService.BuscarPorCorreoAsync(usuario.Correo);

            if (usuarioConMismoCorreo != null &&
                usuarioConMismoCorreo.IdUsuario != usuario.IdUsuario)
            {
                ModelState.AddModelError(
                    "Correo",
                    "Ya existe otro usuario registrado con este correo.");

                return View(usuario);
            }

            await _usuarioService.ActualizarUsuarioAsync(usuario);

            TempData["Mensaje"] = "El usuario fue modificado correctamente.";

            return RedirectToAction(nameof(Index));
        }
    }
}