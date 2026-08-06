using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SistemaBiblioteca.Data;
using SistemaBiblioteca.Services;

namespace SistemaBiblioteca
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuración de Entity Framework
            builder.Services.AddDbContext<BibliotecaContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString(
                        "BibliotecaConnection")));

            // Servicios MVC
            builder.Services.AddControllersWithViews();

            // Servicios de la aplicación
            builder.Services.AddScoped<UsuarioService>();
            builder.Services.AddScoped<PrestamoService>();

            // Configuración de autenticación por cookies
            builder.Services
                .AddAuthentication(
                    CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login/Index";
                    options.AccessDeniedPath = "/Login/AccesoDenegado";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.SlidingExpiration = true;
                });

            var app = builder.Build();

            // Configuración del manejo de errores
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // La autenticación debe ir antes que la autorización
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}