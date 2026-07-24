using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaBiblioteca.Migrations
{
    /// <inheritdoc />
    public partial class CambiarNombreTablaPrestamos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prestamo_Libros_IdLibro",
                table: "Prestamo");

            migrationBuilder.DropForeignKey(
                name: "FK_Prestamo_Usuarios_IdUsuario",
                table: "Prestamo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Prestamo",
                table: "Prestamo");

            migrationBuilder.RenameTable(
                name: "Prestamo",
                newName: "Prestamos");

            migrationBuilder.RenameIndex(
                name: "IX_Prestamo_IdUsuario",
                table: "Prestamos",
                newName: "IX_Prestamos_IdUsuario");

            migrationBuilder.RenameIndex(
                name: "IX_Prestamo_IdLibro",
                table: "Prestamos",
                newName: "IX_Prestamos_IdLibro");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Prestamos",
                table: "Prestamos",
                column: "IdPrestamo");

            migrationBuilder.AddForeignKey(
                name: "FK_Prestamos_Libros_IdLibro",
                table: "Prestamos",
                column: "IdLibro",
                principalTable: "Libros",
                principalColumn: "IdLibro",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prestamos_Usuarios_IdUsuario",
                table: "Prestamos",
                column: "IdUsuario",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prestamos_Libros_IdLibro",
                table: "Prestamos");

            migrationBuilder.DropForeignKey(
                name: "FK_Prestamos_Usuarios_IdUsuario",
                table: "Prestamos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Prestamos",
                table: "Prestamos");

            migrationBuilder.RenameTable(
                name: "Prestamos",
                newName: "Prestamo");

            migrationBuilder.RenameIndex(
                name: "IX_Prestamos_IdUsuario",
                table: "Prestamo",
                newName: "IX_Prestamo_IdUsuario");

            migrationBuilder.RenameIndex(
                name: "IX_Prestamos_IdLibro",
                table: "Prestamo",
                newName: "IX_Prestamo_IdLibro");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Prestamo",
                table: "Prestamo",
                column: "IdPrestamo");

            migrationBuilder.AddForeignKey(
                name: "FK_Prestamo_Libros_IdLibro",
                table: "Prestamo",
                column: "IdLibro",
                principalTable: "Libros",
                principalColumn: "IdLibro",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prestamo_Usuarios_IdUsuario",
                table: "Prestamo",
                column: "IdUsuario",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
