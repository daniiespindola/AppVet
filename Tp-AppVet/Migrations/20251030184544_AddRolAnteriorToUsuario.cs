using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tp_AppVet.Migrations
{
    /// <inheritdoc />
    public partial class AddRolAnteriorToUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RolAnterior",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RolAnterior",
                table: "Usuarios");
        }
    }
}
