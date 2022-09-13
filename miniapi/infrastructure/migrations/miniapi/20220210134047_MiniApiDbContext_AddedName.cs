using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace miniapi.infrastructure.migrations.miniapi
{
    public partial class MiniApiDbContext_AddedName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Items",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Items");
        }
    }
}
