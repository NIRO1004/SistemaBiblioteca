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
                    builder.Configuration.GetConnectionString("BibliotecaConnection")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<Services.LibroService>();
            builder.Services.AddScoped<Services.AutorService>();
            builder.Services.AddScoped<Services.CategoriaService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

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