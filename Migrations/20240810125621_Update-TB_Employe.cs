using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Company_X.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTB_Employe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WindowsName",
                table: "Employes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WindowsName",
                table: "Employes");
        }
    }
}
