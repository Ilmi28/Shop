using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Migrations
{
    public partial class AddProductId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Monitors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Monitors_ProductId",
                table: "Monitors",
                column: "ProductId");

           

            migrationBuilder.AddForeignKey(
                name: "FK_Monitors_Products_ProductId",
                table: "Monitors",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.DropForeignKey(
                name: "FK_Monitors_Products_ProductId",
                table: "Monitors");

            migrationBuilder.DropIndex(
                name: "IX_Monitors_ProductId",
                table: "Monitors");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Monitors");

            

            
        }
    }
}
