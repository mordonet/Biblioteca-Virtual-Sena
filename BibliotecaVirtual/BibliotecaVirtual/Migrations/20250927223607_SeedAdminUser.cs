using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliotecaVirtual.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Correo", "Nombre", "Password", "Rol" },
                values: new object[] { 1, "admin@biblioteca.com", "Administrador Principal", "admin123", "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Correo", "Nombre", "Password", "Rol" },
                values: new object[] { 2, "admin@biblioteca.com", "Administrador Principal", "admin123", "Admin" });
        }
    }
}
