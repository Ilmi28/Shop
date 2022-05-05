using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Migrations.AppIdentityDb
{
    public partial class AddHasProStatusFieldInUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasProStatus",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasProStatus",
                table: "AspNetUsers");
        }
    }
}
