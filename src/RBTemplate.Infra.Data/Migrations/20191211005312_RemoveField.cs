using Microsoft.EntityFrameworkCore.Migrations;

namespace RBTemplate.Infra.Data.Migrations
{
    public partial class RemoveField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Excluido",
                table: "Example");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Excluido",
                table: "Example",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
