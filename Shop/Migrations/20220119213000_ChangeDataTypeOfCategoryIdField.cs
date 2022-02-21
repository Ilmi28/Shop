using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Migrations
{
    public partial class ChangeDataTypeOfCategoryIdField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Monitors_Categories_CategoryId",
                table: "Monitors");

            migrationBuilder.DropIndex(
                name: "IX_Monitors_CategoryId",
                table: "Monitors");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryId",
                table: "Monitors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId1",
                table: "Monitors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Monitors_CategoryId1",
                table: "Monitors",
                column: "CategoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Monitors_Categories_CategoryId1",
                table: "Monitors",
                column: "CategoryId1",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Monitors_Categories_CategoryId1",
                table: "Monitors");

            migrationBuilder.DropIndex(
                name: "IX_Monitors_CategoryId1",
                table: "Monitors");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "Monitors");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Monitors",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Monitors_CategoryId",
                table: "Monitors",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Monitors_Categories_CategoryId",
                table: "Monitors",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
