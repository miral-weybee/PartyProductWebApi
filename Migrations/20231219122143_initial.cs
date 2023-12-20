using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PartyProductWebApi.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dbo.Invoices_dbo.Parties_Party_PartyId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_dbo.Invoices_dbo.Products_Product_ProductId",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "Product_ProductId",
                table: "Invoices",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "Party_PartyId",
                table: "Invoices",
                newName: "PartyId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_Product_ProductId",
                table: "Invoices",
                newName: "IX_Invoices_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_Party_PartyId",
                table: "Invoices",
                newName: "IX_Invoices_PartyId");

            migrationBuilder.RenameColumn(
                name: "Product_ProductId",
                table: "AssignParties",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "Party_PartyId",
                table: "AssignParties",
                newName: "PartyId");

            migrationBuilder.RenameIndex(
                name: "IX_AssignParties_Product_ProductId",
                table: "AssignParties",
                newName: "IX_AssignParties_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_AssignParties_Party_PartyId",
                table: "AssignParties",
                newName: "IX_AssignParties_PartyId");

            migrationBuilder.AddForeignKey(
                name: "FK_dbo.Invoices_dbo.Parties_PartyId",
                table: "Invoices",
                column: "PartyId",
                principalTable: "Parties",
                principalColumn: "PartyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_dbo.Invoices_dbo.Products_ProductId",
                table: "Invoices",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dbo.Invoices_dbo.Parties_PartyId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_dbo.Invoices_dbo.Products_ProductId",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Invoices",
                newName: "Product_ProductId");

            migrationBuilder.RenameColumn(
                name: "PartyId",
                table: "Invoices",
                newName: "Party_PartyId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_ProductId",
                table: "Invoices",
                newName: "IX_Invoices_Product_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_PartyId",
                table: "Invoices",
                newName: "IX_Invoices_Party_PartyId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "AssignParties",
                newName: "Product_ProductId");

            migrationBuilder.RenameColumn(
                name: "PartyId",
                table: "AssignParties",
                newName: "Party_PartyId");

            migrationBuilder.RenameIndex(
                name: "IX_AssignParties_ProductId",
                table: "AssignParties",
                newName: "IX_AssignParties_Product_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_AssignParties_PartyId",
                table: "AssignParties",
                newName: "IX_AssignParties_Party_PartyId");

            migrationBuilder.AddForeignKey(
                name: "FK_dbo.Invoices_dbo.Parties_Party_PartyId",
                table: "Invoices",
                column: "Party_PartyId",
                principalTable: "Parties",
                principalColumn: "PartyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_dbo.Invoices_dbo.Products_Product_ProductId",
                table: "Invoices",
                column: "Product_ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
