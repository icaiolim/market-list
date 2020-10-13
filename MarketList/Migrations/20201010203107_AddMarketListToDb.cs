using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketList.Migrations
{
    public partial class AddMarketListToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdMarketList",
                table: "ProductList",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MarketList",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    DateCreation = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketList", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductList_IdMarketList",
                table: "ProductList",
                column: "IdMarketList");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductList_MarketList_IdMarketList",
                table: "ProductList",
                column: "IdMarketList",
                principalTable: "MarketList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductList_MarketList_IdMarketList",
                table: "ProductList");

            migrationBuilder.DropTable(
                name: "MarketList");

            migrationBuilder.DropIndex(
                name: "IX_ProductList_IdMarketList",
                table: "ProductList");

            migrationBuilder.DropColumn(
                name: "IdMarketList",
                table: "ProductList");
        }
    }
}
