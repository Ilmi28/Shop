using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Migrations.AppIdentityDb
{
    public partial class CartTokenAddToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CartToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CartToken",
                table: "AspNetUsers");
        }
    }
}
