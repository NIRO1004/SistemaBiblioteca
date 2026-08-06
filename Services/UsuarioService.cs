using Microsoft.EntityFrameworkCore;
using SistemaBiblioteca.Data;
using SistemaBiblioteca.Models;

namespace SistemaBiblioteca.Services
{
    public class UsuarioService
    {
        private readonly BibliotecaContext _context;

        public UsuarioService(BibliotecaContext context)
        {
            _context = context;
        }

        // Obtener todos los usuarios
        public async Task<List<Usuario>> ObtenerUsuariosAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // Obtener un usuario por Id
        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.IdUsuario == id);
        }

        // Agregar un usuario
        public async Task AgregarUsuarioAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }

        // Actualizar un usuario
        public async Task ActualizarUsuarioAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

        // Buscar usuarios por nombre
        public async Task<List<Usuario>> BuscarPorNombreAsync(string nombre)
        {
            return await _context.Usuarios
                .Where(u => u.Nombre.Contains(nombre))
                .ToListAsync();
        }

        // Buscar usuario por correo
        public async Task<Usuario?> BuscarPorCorreoAsync(string correo)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == correo);
        }

        // Validar las credenciales para el Login
        public async Task<Usuario?> ValidarCredencialesAsync(string correo, string contrasena)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u =>
                    u.Correo == correo &&
                    u.Contrasena == contrasena);
        }
    }
}