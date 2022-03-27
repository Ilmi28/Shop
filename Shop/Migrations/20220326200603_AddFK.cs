using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Migrations
{
    public partial class AddFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductTableId",
                table: "CartProducts",
                newName: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CartProducts_ProductId",
                table: "CartProducts",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartProducts_Products_ProductId",
                table: "CartProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartProducts_Products_ProductId",
                table: "CartProducts");

            migrationBuilder.DropIndex(
                name: "IX_CartProducts_ProductId",
                table: "CartProducts");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "CartProducts",
                newName: "ProductTableId");
        }
    }
}
