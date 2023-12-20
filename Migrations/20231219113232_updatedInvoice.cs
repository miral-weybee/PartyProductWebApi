using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PartyProductWebApi.Migrations
{
    /// <inheritdoc />
    public partial class updatedInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parties",
                columns: table => new
                {
                    PartyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartyName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.Parties", x => x.PartyId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.Products", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "AssignParties",
                columns: table => new
                {
                    AssignPartyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Party_PartyId = table.Column<int>(type: "int", nullable: false),
                    Product_ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.AssignParties", x => x.AssignPartyId);
                    table.ForeignKey(
                        name: "FK_dbo.AssignParties_dbo.Parties_Party_PartyId",
                        column: x => x.Party_PartyId,
                        principalTable: "Parties",
                        principalColumn: "PartyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.AssignParties_dbo.Products_Product_ProductId",
                        column: x => x.Product_ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrentRate = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Party_PartyId = table.Column<int>(type: "int", nullable: false),
                    Product_ProductId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dbo.Invoices_dbo.Parties_Party_PartyId",
                        column: x => x.Party_PartyId,
                        principalTable: "Parties",
                        principalColumn: "PartyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.Invoices_dbo.Products_Product_ProductId",
                        column: x => x.Product_ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rate = table.Column<int>(type: "int", nullable: false),
                    DateOfRate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ProductName_ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.ProductRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dbo.ProductRates_dbo.Products_ProductName_ProductId",
                        column: x => x.ProductName_ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignParties_Party_PartyId",
                table: "AssignParties",
                column: "Party_PartyId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignParties_Product_ProductId",
                table: "AssignParties",
                column: "Product_ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_Party_PartyId",
                table: "Invoices",
                column: "Party_PartyId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_Product_ProductId",
                table: "Invoices",
                column: "Product_ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRates_ProductName_ProductId",
                table: "ProductRates",
                column: "ProductName_ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignParties");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "ProductRates");

            migrationBuilder.DropTable(
                name: "Parties");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
