using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RBTemplate.Infra.CrossCutting.Identity.Migrations.RefreshToken
{
    public partial class AddExpirationDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "RefreshToken",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "RefreshToken");
        }
    }
}
